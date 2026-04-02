// Presentation/Controllers/HomeController.cs
using Microsoft.AspNetCore.Mvc;
using ClinicaMVC.Domain.Factories;

namespace ClinicaMVC.Controllers;

public class HomeController : Controller
{
    private readonly AtendimentoService _AtendimentoService;

    public HomeController(AtendimentoService atendimentoService)
        => _AtendimentoService = atendimentoService;

    public async Task<IActionResult> Index()
    {
        var pacientes = await _AtendimentoService.ListarPacientes();
        var medicos = await _AtendimentoService.ListarMedicos();
        var atendimentos = await _AtendimentoService.ListarAtendimentos();
        var horarios = _AtendimentoService.ListarHorariosDisponiveis();

        ViewBag.TotalPacientes = pacientes.Count();
        ViewBag.TotalMedicos = medicos.Count();
        ViewBag.TotalAtendimentos = atendimentos.Count();
        ViewBag.HorariosDisponiveis = horarios.Count;
        ViewBag.UltimosAtendimentos = atendimentos.OrderByDescending(a => a.DataHora).Take(5);

        return View();
    }
}
