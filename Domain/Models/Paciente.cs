// Domain/Models/Paciente.cs
namespace ClinicaMVC.Domain.Models;

public class Paciente
{
    public string Id { get; private set; }
    public string Nome { get; private set; }
    public DateTime DataNascimento { get; private set; }
    public List<string> HistoricoConsultas { get; private set; }

    public Paciente(string nome, DateTime dataNascimento)
    {
        Id = Guid.NewGuid().ToString();
        Nome = nome;
        DataNascimento = dataNascimento;
        HistoricoConsultas = new List<string>();
    }

    // EF Core constructor
    protected Paciente()
    {
        Id = string.Empty;
        Nome = string.Empty;
        HistoricoConsultas = new List<string>();
    }

    public string GetId() => Id;
    public string GetNome() => Nome;
    public List<string> GetHistorico() => HistoricoConsultas;
    public void AdicionarHistorico(string registro) => HistoricoConsultas.Add(registro);
}
