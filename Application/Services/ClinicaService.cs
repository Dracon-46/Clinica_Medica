// Application/Services/ClinicaService.cs
using ClinicaMVC.Domain.Factories;
using ClinicaMVC.Domain.Interfaces;
using ClinicaMVC.Domain.Models;

namespace ClinicaMVC.Application.Services;

/// <summary>
/// ClinicaService — orquestra os casos de uso da clínica (SRP, DIP)
/// </summary>
public class ClinicaService
{
    private readonly IPacienteRepository _pacienteRepo;
    private readonly IMedicoRepository   _medicoRepo;
    private readonly IAtendimentoRepository _atendimentoRepo;

    public ClinicaService(
        IPacienteRepository pacienteRepo,
        IMedicoRepository medicoRepo,
        IAtendimentoRepository atendimentoRepo)
    {
        _pacienteRepo    = pacienteRepo;
        _medicoRepo      = medicoRepo;
        _atendimentoRepo = atendimentoRepo;
    }

    public async Task AgendarConsulta(
        string pacienteId, int medicoId,
        string tipoConsulta, string tipoPacote, DateTime dataHora)
    {
        var factory = ConsultaFactoryResolver.Resolver(tipoConsulta);

        if (!factory.ValidarHorario(dataHora))
            throw new InvalidOperationException(
                $"Horário {dataHora:HH:mm} inválido para {tipoConsulta}.");

        // Valida existência
        var paciente = await _pacienteRepo.GetByIdAsync(pacienteId)
            ?? throw new KeyNotFoundException("Paciente não encontrado.");
        var medico = await _medicoRepo.GetByIdAsync(medicoId)
            ?? throw new KeyNotFoundException("Médico não encontrado.");

        // Demonstra Abstract Factory
        var pacoteFactory = PacoteFactoryResolver.Resolver(tipoPacote);
        pacoteFactory.MontarPacote(); // executa a lógica de negócio

        var atendimento = new Atendimento
        {
            PacienteId   = pacienteId,
            MedicoId     = medicoId,
            TipoConsulta = tipoConsulta,
            TipoPacote   = tipoPacote,
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
    {
        var paciente = new Paciente(nome, dataNascimento);
        await _pacienteRepo.AddAsync(paciente);
    }

    public async Task CadastrarMedico(string crm, string nome, string especialidade)
    {
        var medico = new Medico(crm, nome, especialidade);
        await _medicoRepo.AddAsync(medico);
    }
}
