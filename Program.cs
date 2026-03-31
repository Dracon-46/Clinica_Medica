// Program.cs
using Microsoft.EntityFrameworkCore;
using ClinicaMVC.Infrastructure.Data;
using ClinicaMVC.Infrastructure.Repositories;
using ClinicaMVC.Domain.Interfaces;
using ClinicaMVC.Domain.Factories;

var builder = WebApplication.CreateBuilder(args);

// ── MVC ──────────────────────────────────────────────────────────────────
builder.Services.AddControllersWithViews();

// ── EF Core + SQLite ─────────────────────────────────────────────────────
builder.Services.AddDbContext<ClinicaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")
        ?? "Data Source=clinica.db"));

// ── Repositórios — DIP: controllers/services dependem de interfaces ───────
builder.Services.AddScoped<IPacienteRepository,    PacienteRepository>();
builder.Services.AddScoped<IMedicoRepository,      MedicoRepository>();
builder.Services.AddScoped<IAtendimentoRepository, AtendimentoRepository>();

// ── Abstract Factory — DIP: registra a abstração, injeta um concreto ──────
// Para mudar o pacote padrão de toda a aplicação, basta trocar esta linha.
// Nenhuma outra classe precisa ser alterada. Isso é OCP em ação.
builder.Services.AddScoped<PacoteAtendimento, PacoteBasicoFactory>();

// ── Application Service ───────────────────────────────────────────────────
builder.Services.AddScoped<Atendimento2>();

var app = builder.Build();

// ── Criar banco automaticamente ───────────────────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();
    db.Database.EnsureCreated();
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
