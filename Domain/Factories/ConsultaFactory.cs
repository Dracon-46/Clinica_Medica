// Domain/Factories/ConsultaFactory.cs
using ClinicaMVC.Domain.Interfaces;
using ClinicaMVC.Domain.Models.Consultas;

namespace ClinicaMVC.Domain.Factories;

/// <summary>
/// Factory Method — define contrato para criação de consultas (OCP, DIP)
/// </summary>
public abstract class ConsultaFactory
{
    public abstract IConsulta CriarConsulta();
    public abstract string GetDescricao();
    public abstract bool ValidarHorario(DateTime horario);
}

public class GeralFactory : ConsultaFactory
{
    public override IConsulta CriarConsulta() => new ConsultaGeralImpl();
    public override string GetDescricao() => "Consulta de Clínica Geral";
    public override bool ValidarHorario(DateTime horario)
    {
        GerenciadorAgenda.GetInstancia().ListarHorarios();
        return horario.Hour >= 8 && horario.Hour <= 18;
    }
}

public class PediatriaFactory : ConsultaFactory
{
    public override IConsulta CriarConsulta() => new ConsultaPediatriaImpl();
    public override string GetDescricao() => "Consulta de Pediatria";
    public override bool ValidarHorario(DateTime horario)
    {
        GerenciadorAgenda.GetInstancia().ListarHorarios();
        return horario.Hour >= 9 && horario.Hour <= 17;
    }
}

public class OrtopediaFactory : ConsultaFactory
{
    public override IConsulta CriarConsulta() => new ConsultaOrtopediaImpl();
    public override string GetDescricao() => "Consulta de Ortopedia";
    public override bool ValidarHorario(DateTime horario)
    {
        GerenciadorAgenda.GetInstancia().ListarHorarios();
        return horario.Hour >= 7 && horario.Hour <= 19;
    }
}

/// <summary>
/// Resolver — retorna a factory correta para a especialidade (OCP)
/// </summary>
public static class ConsultaFactoryResolver
{
    public static ConsultaFactory Resolver(string tipo) => tipo.ToLower() switch
    {
        "geral"      => new GeralFactory(),
        "pediatria"  => new PediatriaFactory(),
        "ortopedia"  => new OrtopediaFactory(),
        _ => throw new ArgumentException($"Especialidade '{tipo}' não suportada.")
    };
}
