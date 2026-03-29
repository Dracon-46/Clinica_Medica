// Infrastructure/Repositories/Repositories.cs
using Microsoft.EntityFrameworkCore;
using ClinicaMVC.Domain.Interfaces;
using ClinicaMVC.Domain.Models;
using ClinicaMVC.Infrastructure.Data;

namespace ClinicaMVC.Infrastructure.Repositories;

public class PacienteRepository : IPacienteRepository
{
    private readonly ClinicaDbContext _ctx;
    public PacienteRepository(ClinicaDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Paciente>> GetAllAsync() =>
        await _ctx.Pacientes.ToListAsync();

    public async Task<Paciente?> GetByIdAsync(string id) =>
        await _ctx.Pacientes.FindAsync(id);

    public async Task AddAsync(Paciente p)
    {
        await _ctx.Pacientes.AddAsync(p);
        await _ctx.SaveChangesAsync();
    }

    public async Task UpdateAsync(Paciente p)
    {
        _ctx.Pacientes.Update(p);
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var p = await GetByIdAsync(id);
        if (p is not null) { _ctx.Pacientes.Remove(p); await _ctx.SaveChangesAsync(); }
    }
}

public class MedicoRepository : IMedicoRepository
{
    private readonly ClinicaDbContext _ctx;
    public MedicoRepository(ClinicaDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Medico>> GetAllAsync() =>
        await _ctx.Medicos.ToListAsync();

    public async Task<Medico?> GetByIdAsync(int id) =>
        await _ctx.Medicos.FindAsync(id);

    public async Task AddAsync(Medico m)
    {
        await _ctx.Medicos.AddAsync(m);
        await _ctx.SaveChangesAsync();
    }

    public async Task UpdateAsync(Medico m)
    {
        _ctx.Medicos.Update(m);
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var m = await GetByIdAsync(id);
        if (m is not null) { _ctx.Medicos.Remove(m); await _ctx.SaveChangesAsync(); }
    }
}

public class AtendimentoRepository : IAtendimentoRepository
{
    private readonly ClinicaDbContext _ctx;
    public AtendimentoRepository(ClinicaDbContext ctx) => _ctx = ctx;

    public async Task<IEnumerable<Atendimento>> GetAllAsync() =>
        await _ctx.Atendimentos
            .Include(a => a.Paciente)
            .Include(a => a.Medico)
            .ToListAsync();

    public async Task<Atendimento?> GetByIdAsync(int id) =>
        await _ctx.Atendimentos
            .Include(a => a.Paciente)
            .Include(a => a.Medico)
            .FirstOrDefaultAsync(a => a.Id == id);

    public async Task AddAsync(Atendimento a)
    {
        await _ctx.Atendimentos.AddAsync(a);
        await _ctx.SaveChangesAsync();
    }

    public async Task UpdateAsync(Atendimento a)
    {
        _ctx.Atendimentos.Update(a);
        await _ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var a = await GetByIdAsync(id);
        if (a is not null) { _ctx.Atendimentos.Remove(a); await _ctx.SaveChangesAsync(); }
    }

    public async Task<IEnumerable<Atendimento>> GetByPacienteAsync(string pacienteId) =>
        await _ctx.Atendimentos.Include(a => a.Medico)
            .Where(a => a.PacienteId == pacienteId).ToListAsync();

    public async Task<IEnumerable<Atendimento>> GetByMedicoAsync(int medicoId) =>
        await _ctx.Atendimentos.Include(a => a.Paciente)
            .Where(a => a.MedicoId == medicoId).ToListAsync();
}
