// Domain/Factories/PacoteAtendimento.cs
using ClinicaMVC.Domain.Interfaces;
using ClinicaMVC.Domain.Models;
using ClinicaMVC.Domain.Models.Consultas;
using ClinicaMVC.Domain.Factories;

namespace ClinicaMVC.Domain.Factories;

/// <summary>
/// Abstract Factory — cria famílias de objetos: IConsulta + IExame + IReceituario
/// </summary>
public abstract class PacoteAtendimento
{
    public abstract IConsulta CriarConsulta();
    public abstract IExame CriarExame();
    public abstract IReceituario CriarReceituario();

    public void MontarPacote()
    {
        CriarConsulta().Realizar();
        CriarExame().Solicitar();
        CriarReceituario().Emitir();
    }

    public virtual string GetDescricao() => GetType().Name;
}

public class PacoteBasicoFactory : PacoteAtendimento
{
    public override IConsulta CriarConsulta() => new ConsultaGeralImpl();
    public override IExame CriarExame() => new ExameBasico();
    public override IReceituario CriarReceituario() => new ReceituarioSimples();
    public override string GetDescricao() => "Pacote Básico";
}

public class PacoteCompletoFactory : PacoteAtendimento
{
    public override IConsulta CriarConsulta() => new ConsultaGeralImpl();
    public override IExame CriarExame() => new ExameCompleto();
    public override IReceituario CriarReceituario() => new ReceituarioDetalhado();
    public override string GetDescricao() => "Pacote Completo";
}

public static class PacoteFactoryResolver
{
    public static PacoteAtendimento Resolver(string tipo) => tipo.ToLower() switch
    {
        "basico" => new PacoteBasicoFactory(),
        "completo" => new PacoteCompletoFactory(),
        _ => throw new ArgumentException($"Pacote '{tipo}' não encontrado.")
    };
}

public class AtendimentoService
{

    private readonly IPacienteRepository _pacienteRepo;
    private readonly IMedicoRepository _medicoRepo;
    private readonly IAtendimentoRepository _atendimentoRepo;

    // ← injetado conforme o diagrama: "-pacoteFactory PacoteAtendimento"
    // DIP: depende da abstração, não do PacoteFactoryResolver concreto
    private readonly PacoteAtendimento _pacoteFactory;
    public AtendimentoService(
    IPacienteRepository pacienteRepo,
    IMedicoRepository medicoRepo,
    IAtendimentoRepository atendimentoRepo,
    PacoteAtendimento pacoteFactory)      // << injeção via DI
    {
        _pacienteRepo = pacienteRepo;
        _medicoRepo = medicoRepo;
        _atendimentoRepo = atendimentoRepo;
        _pacoteFactory = pacoteFactory;
    }

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
            PacienteId = pacienteId,
            MedicoId = medicoId,
            TipoConsulta = tipoConsulta,
            TipoPacote = pacote.GetDescricao(),
            DataHora = dataHora,
            Status = "Agendado"
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