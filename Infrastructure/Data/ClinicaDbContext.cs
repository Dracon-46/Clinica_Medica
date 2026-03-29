// Infrastructure/Data/ClinicaDbContext.cs
using Microsoft.EntityFrameworkCore;
using ClinicaMVC.Domain.Models;

namespace ClinicaMVC.Infrastructure.Data;

public class ClinicaDbContext : DbContext
{
    public ClinicaDbContext(DbContextOptions<ClinicaDbContext> options) : base(options) { }

    public DbSet<Paciente>     Pacientes    => Set<Paciente>();
    public DbSet<Medico>       Medicos      => Set<Medico>();
    public DbSet<Atendimento>  Atendimentos => Set<Atendimento>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // ── Paciente ──────────────────────────────────────────
        modelBuilder.Entity<Paciente>(e =>
        {
            e.HasKey(p => p.Id);
            e.Property(p => p.Nome).IsRequired().HasMaxLength(120);
            e.Property(p => p.DataNascimento).IsRequired();
            e.Ignore(p => p.HistoricoConsultas);
        });

        // ── Medico ────────────────────────────────────────────
        modelBuilder.Entity<Medico>(e =>
        {
            e.HasKey(m => m.Id);
            e.Property(m => m.Crm).IsRequired().HasMaxLength(20);
            e.Property(m => m.Nome).IsRequired().HasMaxLength(120);
            e.Property(m => m.Especialidade).IsRequired().HasMaxLength(60);
            e.HasIndex(m => m.Crm).IsUnique();
        });

        // ── Atendimento ───────────────────────────────────────
        modelBuilder.Entity<Atendimento>(e =>
        {
            e.HasKey(a => a.Id);
            e.Property(a => a.TipoPacote).IsRequired().HasMaxLength(30);
            e.Property(a => a.TipoConsulta).IsRequired().HasMaxLength(30);
            e.Property(a => a.Status).IsRequired().HasMaxLength(20);
            e.HasOne(a => a.Paciente)
             .WithMany()
             .HasForeignKey(a => a.PacienteId)
             .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(a => a.Medico)
             .WithMany()
             .HasForeignKey(a => a.MedicoId)
             .OnDelete(DeleteBehavior.Restrict);
        });

        // ── Seed Data ─────────────────────────────────────────
        modelBuilder.Entity<Medico>().HasData(
            new { Id = 1, Crm = "CRM-12345", Nome = "Dr. Carlos Andrade", Especialidade = "Geral" },
            new { Id = 2, Crm = "CRM-67890", Nome = "Dra. Ana Lima",      Especialidade = "Pediatria" },
            new { Id = 3, Crm = "CRM-11223", Nome = "Dr. Roberto Souza",  Especialidade = "Ortopedia" }
        );
    }
}
