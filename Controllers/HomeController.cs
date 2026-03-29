// Presentation/Controllers/HomeController.cs
using Microsoft.AspNetCore.Mvc;
using ClinicaMVC.Application.Services;

namespace ClinicaMVC.Controllers;

public class HomeController : Controller
{
    private readonly ClinicaService _clinicaService;

    public HomeController(ClinicaService clinicaService)
        => _clinicaService = clinicaService;

    public async Task<IActionResult> Index()
    {
        var pacientes = await _clinicaService.ListarPacientes();
        var medicos = await _clinicaService.ListarMedicos();
        var atendimentos = await _clinicaService.ListarAtendimentos();
        var horarios = _clinicaService.ListarHorariosDisponiveis();

        ViewBag.TotalPacientes = pacientes.Count();
        ViewBag.TotalMedicos = medicos.Count();
        ViewBag.TotalAtendimentos = atendimentos.Count();
        ViewBag.HorariosDisponiveis = horarios.Count;
        ViewBag.UltimosAtendimentos = atendimentos.OrderByDescending(a => a.DataHora).Take(5);

        return View();
    }
}
