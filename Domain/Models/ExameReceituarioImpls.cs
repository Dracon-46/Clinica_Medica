// Domain/Models/ExameImpl.cs
using ClinicaMVC.Domain.Interfaces;

namespace ClinicaMVC.Domain.Models;

public class ExameBasico : IExame
{
    private readonly string _tipo = "Exame Básico";
    public void Solicitar() => Console.WriteLine($"Solicitando {_tipo}");
    public string GetTipo() => _tipo;
}

public class ExameCompleto : IExame
{
    private readonly string _tipo = "Exame Completo";
    public void Solicitar() => Console.WriteLine($"Solicitando {_tipo}");
    public string GetTipo() => _tipo;
}

public class ReceituarioSimples : IReceituario
{
    private readonly string _prescricao = "Receituário Simples";
    public void Emitir() => Console.WriteLine($"Emitindo {_prescricao}");
    public string GetPrescricao() => _prescricao;
}

public class ReceituarioDetalhado : IReceituario
{
    private readonly string _prescricao = "Receituário Detalhado";
    public void Emitir() => Console.WriteLine($"Emitindo {_prescricao}");
    public string GetPrescricao() => _prescricao;
}
