// Program.cs
using Microsoft.EntityFrameworkCore;
using ClinicaMVC.Infrastructure.Data;
using ClinicaMVC.Infrastructure.Repositories;
using ClinicaMVC.Domain.Interfaces;
using ClinicaMVC.Application.Services;

var builder = WebApplication.CreateBuilder(args);

// ── MVC ──────────────────────────────────────────────
builder.Services.AddControllersWithViews();

// ── EF Core + SQLite ──────────────────────────────────
builder.Services.AddDbContext<ClinicaDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("Default")
        ?? "Data Source=clinica.db"));

// ── Repositórios (DIP) ────────────────────────────────
builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<IMedicoRepository, MedicoRepository>();
builder.Services.AddScoped<IAtendimentoRepository, AtendimentoRepository>();

// ── Application Services ──────────────────────────────
builder.Services.AddScoped<ClinicaService>();

var app = builder.Build();

// ── Criar banco automaticamente (EnsureCreated cria tabelas pelo modelo) ──
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ClinicaDbContext>();
    db.Database.EnsureCreated(); // cria tabelas se não existirem + aplica Seed Data
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
