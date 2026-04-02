// Presentation/Controllers/AgendaController.cs
using Microsoft.AspNetCore.Mvc;
using ClinicaMVC.Domain.Factories;

namespace ClinicaMVC.Controllers;

public class AgendaController : Controller
{
    private readonly AtendimentoService _service;
    public AgendaController(AtendimentoService service) => _service = service;

    public async Task<IActionResult> Index()
    {
        var atendimentos = await _service.ListarAtendimentos();
        return View(atendimentos);
    }

    public async Task<IActionResult> Agendar()
    {
        ViewBag.Pacientes = await _service.ListarPacientes();
        ViewBag.Medicos = await _service.ListarMedicos();
        ViewBag.Horarios = _service.ListarHorariosDisponiveis();
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Agendar(string pacienteId, int medicoId,
        string tipoConsulta, string tipoPacote, DateTime dataHora)
    {
        try
        {
            await _service.AgendarConsulta(pacienteId, medicoId, tipoConsulta, tipoPacote, dataHora);
            TempData["Sucesso"] = "Consulta agendada com sucesso!";
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Erro"] = ex.Message;
            ViewBag.Pacientes = await _service.ListarPacientes();
            ViewBag.Medicos = await _service.ListarMedicos();
            ViewBag.Horarios = _service.ListarHorariosDisponiveis();
            return View();
        }
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Cancelar(int id)
    {
        try
        {
            await _service.CancelarConsulta(id);
            TempData["Sucesso"] = "Consulta cancelada.";
        }
        catch (Exception ex)
        {
            TempData["Erro"] = ex.Message;
        }
        return RedirectToAction(nameof(Index));
    }
}
