// Application/Services/ClinicaService.cs
using ClinicaMVC.Domain.Factories;
using ClinicaMVC.Domain.Interfaces;
using ClinicaMVC.Domain.Models;

namespace ClinicaMVC.Application.Services;

/// <summary>
/// ClinicaService — orquestra os casos de uso da clínica.
/// 
/// SOLID aplicado:
///   SRP — só coordena casos de uso, não conhece detalhes de persistência nem de criação de objetos.
///   DIP — depende de IPacienteRepository, IMedicoRepository, IAtendimentoRepository (abstrações)
///         e de PacoteAtendimento (abstração da Abstract Factory), nunca de concretos.
///   OCP — para trocar o pacote padrão basta mudar o registro no Program.cs, sem tocar aqui.
/// </summary>
public class ClinicaService
{
    private readonly IPacienteRepository    _pacienteRepo;
    private readonly IMedicoRepository      _medicoRepo;
    private readonly IAtendimentoRepository _atendimentoRepo;

    // ← injetado conforme o diagrama: "-pacoteFactory PacoteAtendimento"
    // DIP: depende da abstração, não do PacoteFactoryResolver concreto
    private readonly PacoteAtendimento _pacoteFactory;

    public ClinicaService(
        IPacienteRepository    pacienteRepo,
        IMedicoRepository      medicoRepo,
        IAtendimentoRepository atendimentoRepo,
        PacoteAtendimento      pacoteFactory)      // << injeção via DI
    {
        _pacienteRepo    = pacienteRepo;
        _medicoRepo      = medicoRepo;
        _atendimentoRepo = atendimentoRepo;
        _pacoteFactory   = pacoteFactory;
    }

    /// <summary>
    /// Agenda uma consulta usando a factory de consulta correta (Factory Method)
    /// e o pacote injetado (Abstract Factory).
    /// O tipo de pacote ainda pode ser sobrescrito por parâmetro quando necessário.
    /// </summary>
    public async Task AgendarConsulta(
        string pacienteId, int medicoId,
        string tipoConsulta, string tipoPacote, DateTime dataHora)
    {
        // Factory Method — valida horário conforme especialidade
        var consultaFactory = ConsultaFactoryResolver.Resolver(tipoConsulta);
        if (!consultaFactory.ValidarHorario(dataHora))
            throw new InvalidOperationException(
                $"Horário {dataHora:HH:mm} inválido para a especialidade '{tipoConsulta}'.");

        // Valida existência das entidades
        _ = await _pacienteRepo.GetByIdAsync(pacienteId)
            ?? throw new KeyNotFoundException("Paciente não encontrado.");
        _ = await _medicoRepo.GetByIdAsync(medicoId)
            ?? throw new KeyNotFoundException("Médico não encontrado.");

        // Abstract Factory — usa o pacote injetado ou resolve um específico se informado
        var pacote = tipoPacote.ToLower() == _pacoteFactory.GetDescricao().ToLower()
            ? _pacoteFactory
            : PacoteFactoryResolver.Resolver(tipoPacote);

        pacote.MontarPacote(); // executa lógica de negócio do pacote

        var atendimento = new Atendimento
        {
            PacienteId   = pacienteId,
            MedicoId     = medicoId,
            TipoConsulta = tipoConsulta,
            TipoPacote   = pacote.GetDescricao(),
            DataHora     = dataHora,
            Status       = "Agendado"
        };

        await _atendimentoRepo.AddAsync(atendimento);
        GerenciadorAgenda.GetInstancia().RegistrarConsulta(atendimento.Id, atendimento.DataHora);
    }

    public async Task CancelarConsulta(int atendimentoId)
    {
        var atendimento = await _atendimentoRepo.GetByIdAsync(atendimentoId)
            ?? throw new KeyNotFoundException($"Atendimento {atendimentoId} não encontrado.");

        atendimento.Status = "Cancelado";
        await _atendimentoRepo.UpdateAsync(atendimento);
        GerenciadorAgenda.GetInstancia().CancelarConsulta(atendimentoId, atendimento.DataHora);
    }

    public List<DateTime> ListarHorariosDisponiveis() =>
        GerenciadorAgenda.GetInstancia().ListarHorarios();

    public async Task<IEnumerable<Atendimento>> ListarAtendimentos() =>
        await _atendimentoRepo.GetAllAsync();

    public async Task<IEnumerable<Paciente>> ListarPacientes() =>
        await _pacienteRepo.GetAllAsync();

    public async Task<IEnumerable<Medico>> ListarMedicos() =>
        await _medicoRepo.GetAllAsync();

    public async Task CadastrarPaciente(string nome, DateTime dataNascimento)
        => await _pacienteRepo.AddAsync(new Paciente(nome, dataNascimento));

    public async Task CadastrarMedico(string crm, string nome, string especialidade)
        => await _medicoRepo.AddAsync(new Medico(crm, nome, especialidade));
}
