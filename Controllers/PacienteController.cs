// Presentation/Controllers/PacienteController.cs
using Microsoft.AspNetCore.Mvc;
using ClinicaMVC.Domain.Factories;

namespace ClinicaMVC.Controllers;

public class PacienteController : Controller
{
    private readonly Atendimento2 _service;
    public PacienteController(Atendimento2 service) => _service = service;

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
