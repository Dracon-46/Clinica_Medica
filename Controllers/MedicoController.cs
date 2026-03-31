// Presentation/Controllers/MedicoController.cs
using Microsoft.AspNetCore.Mvc;
using ClinicaMVC.Domain.Factories;

namespace ClinicaMVC.Controllers;

public class MedicoController : Controller
{
    private readonly Atendimento2 _service;
    public MedicoController(Atendimento2 service) => _service = service;

    public async Task<IActionResult> Index()
    {
        var medicos = await _service.ListarMedicos();
        return View(medicos);
    }

    public IActionResult Criar() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Criar(string crm, string nome, string especialidade)
    {
        if (string.IsNullOrWhiteSpace(crm) || string.IsNullOrWhiteSpace(nome))
        {
            ModelState.AddModelError("", "CRM e Nome são obrigatórios.");
            return View();
        }
        await _service.CadastrarMedico(crm, nome, especialidade);
        TempData["Sucesso"] = $"Médico '{nome}' cadastrado com sucesso!";
        return RedirectToAction(nameof(Index));
    }
}
