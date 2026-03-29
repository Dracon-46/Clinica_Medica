// Domain/Factories/PacoteAtendimento.cs
using ClinicaMVC.Domain.Interfaces;
using ClinicaMVC.Domain.Models;
using ClinicaMVC.Domain.Models.Consultas;

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
    public override IConsulta CriarConsulta()      => new ConsultaGeralImpl();
    public override IExame CriarExame()            => new ExameBasico();
    public override IReceituario CriarReceituario() => new ReceituarioSimples();
    public override string GetDescricao()           => "Pacote Básico";
}

public class PacoteCompletoFactory : PacoteAtendimento
{
    public override IConsulta CriarConsulta()      => new ConsultaGeralImpl();
    public override IExame CriarExame()            => new ExameCompleto();
    public override IReceituario CriarReceituario() => new ReceituarioDetalhado();
    public override string GetDescricao()           => "Pacote Completo";
}

public static class PacoteFactoryResolver
{
    public static PacoteAtendimento Resolver(string tipo) => tipo.ToLower() switch
    {
        "basico"   => new PacoteBasicoFactory(),
        "completo" => new PacoteCompletoFactory(),
        _ => throw new ArgumentException($"Pacote '{tipo}' não encontrado.")
    };
}
