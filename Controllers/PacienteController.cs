// Presentation/Controllers/PacienteController.cs
using Microsoft.AspNetCore.Mvc;
using ClinicaMVC.Application.Services;

namespace ClinicaMVC.Controllers;

public class PacienteController : Controller
{
    private readonly ClinicaService _service;
    public PacienteController(ClinicaService service) => _service = service;

    public async Task<IActionResult> Index()
    {
        var pacientes = await _service.ListarPacientes();
        return View(pacientes);
    }

    public IActionResult Criar() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(string nome, DateTime dataNascimento)
    {
        if (string.IsNullOrWhiteSpace(nome))
        {
            ModelState.AddModelError("", "Nome é obrigatório.");
            return View();
        }
        await _service.CadastrarPaciente(nome, dataNascimento);
        TempData["Sucesso"] = $"Paciente '{nome}' cadastrado com sucesso!";
        return RedirectToAction(nameof(Index));
    }
}
