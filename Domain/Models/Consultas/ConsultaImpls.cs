// Domain/Models/Consultas/ConsultaGeralImpl.cs
using ClinicaMVC.Domain.Interfaces;

namespace ClinicaMVC.Domain.Models.Consultas;

public class ConsultaGeralImpl : IConsulta
{
    private readonly string _especialidade = "Clínica Geral";

    public void Realizar()
    {
        Console.WriteLine($"Realizando consulta de {_especialidade}");
    }

    public string GetEspecialidade() => _especialidade;
}

public class ConsultaPediatriaImpl : IConsulta
{
    private readonly string _especialidade = "Pediatria";

    public void Realizar()
    {
        Console.WriteLine($"Realizando consulta de {_especialidade}");
    }

    public string GetEspecialidade() => _especialidade;
}

public class ConsultaOrtopediaImpl : IConsulta
{
    private readonly string _especialidade = "Ortopedia";

    public void Realizar()
    {
        Console.WriteLine($"Realizando consulta de {_especialidade}");
    }

    public string GetEspecialidade() => _especialidade;
}
