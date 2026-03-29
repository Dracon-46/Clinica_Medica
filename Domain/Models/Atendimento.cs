// Domain/Models/Atendimento.cs
namespace ClinicaMVC.Domain.Models;

public class Atendimento
{
    public int Id { get; set; }
    public string PacienteId { get; set; } = string.Empty;
    public int MedicoId { get; set; }
    public string TipoPacote { get; set; } = string.Empty;
    public string TipoConsulta { get; set; } = string.Empty;
    public DateTime DataHora { get; set; }
    public string Status { get; set; } = "Agendado";

    // Navigation properties (EF Core)
    public Paciente? Paciente { get; set; }
    public Medico? Medico { get; set; }
}
