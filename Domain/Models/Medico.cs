// Domain/Models/Medico.cs
namespace ClinicaMVC.Domain.Models;

public class Medico
{
    public int Id { get; set; }
    public string Crm { get; private set; }
    public string Nome { get; private set; }
    public string Especialidade { get; private set; }

    public Medico(string crm, string nome, string especialidade)
    {
        Crm = crm;
        Nome = nome;
        Especialidade = especialidade;
    }

    protected Medico()
    {
        Crm = string.Empty;
        Nome = string.Empty;
        Especialidade = string.Empty;
    }

    public string GetCrm() => Crm;
    public string GetNome() => Nome;
    public string GetEspecialidade() => Especialidade;
}
