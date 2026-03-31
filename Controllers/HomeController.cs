// Presentation/Controllers/HomeController.cs
using Microsoft.AspNetCore.Mvc;
using ClinicaMVC.Domain.Factories;

namespace ClinicaMVC.Controllers;

public class HomeController : Controller
{
    private readonly Atendimento2 _Atendimento2;

    public HomeController(Atendimento2 atendimento2)
        => _Atendimento2 = atendimento2;

    public async Task<IActionResult> Index()
    {
        var pacientes = await _Atendimento2.ListarPacientes();
        var medicos = await _Atendimento2.ListarMedicos();
        var atendimentos = await _Atendimento2.ListarAtendimentos();
        var horarios = _Atendimento2.ListarHorariosDisponiveis();

        ViewBag.TotalPacientes = pacientes.Count();
        ViewBag.TotalMedicos = medicos.Count();
        ViewBag.TotalAtendimentos = atendimentos.Count();
        ViewBag.HorariosDisponiveis = horarios.Count;
        ViewBag.UltimosAtendimentos = atendimentos.OrderByDescending(a => a.DataHora).Take(5);

        return View();
    }
}
