// Domain/Factories/GerenciadorAgenda.cs
namespace ClinicaMVC.Domain.Factories;

/// <summary>
/// Singleton thread-safe — instância única do gerenciador de agenda (SRP + Singleton)
/// </summary>
public sealed class GerenciadorAgenda
{
    private static GerenciadorAgenda? _instancia;
    private static readonly object _lock = new();

    private readonly List<DateTime> _horariosDisponiveis;
    private readonly HashSet<int> _idsAgendados;

    private GerenciadorAgenda()
    {
        _horariosDisponiveis = GerarHorariosDisponiveis();
        _idsAgendados = new HashSet<int>();
    }

    public static GerenciadorAgenda GetInstancia()
    {
        if (_instancia is null)
            lock (_lock)
                _instancia ??= new GerenciadorAgenda();
        return _instancia;
    }

    public void RegistrarConsulta(int atendimentoId, DateTime horario)
    {
        _idsAgendados.Add(atendimentoId);
        _horariosDisponiveis.Remove(horario);
    }

    public bool CancelarConsulta(int atendimentoId, DateTime horario)
    {
        if (!_idsAgendados.Remove(atendimentoId)) return false;
        _horariosDisponiveis.Add(horario);
        _horariosDisponiveis.Sort();
        return true;
    }

    public List<DateTime> ListarHorarios() => new(_horariosDisponiveis);

    public bool HorarioDisponivel(DateTime horario) => _horariosDisponiveis.Contains(horario);

    private static List<DateTime> GerarHorariosDisponiveis()
    {
        var horarios = new List<DateTime>();
        var hoje = DateTime.Today;
        for (int dia = 0; dia < 14; dia++)
        {
            var data = hoje.AddDays(dia);
            if (data.DayOfWeek is DayOfWeek.Saturday or DayOfWeek.Sunday) continue;
            for (int hora = 8; hora <= 17; hora++)
                horarios.Add(data.AddHours(hora));
        }
        return horarios;
    }
}
