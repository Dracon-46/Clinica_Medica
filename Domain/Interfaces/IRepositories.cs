// Domain/Interfaces/IRepositories.cs
using ClinicaMVC.Domain.Models;

namespace ClinicaMVC.Domain.Interfaces;

public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync();
    Task<T?> GetByIdAsync(int id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(int id);
}

public interface IPacienteRepository
{
    Task<IEnumerable<Paciente>> GetAllAsync();
    Task<Paciente?> GetByIdAsync(string id);
    Task AddAsync(Paciente paciente);
    Task UpdateAsync(Paciente paciente);
    Task DeleteAsync(string id);
}

public interface IMedicoRepository : IRepository<Medico> { }

public interface IAtendimentoRepository : IRepository<Atendimento>
{
    Task<IEnumerable<Atendimento>> GetByPacienteAsync(string pacienteId);
    Task<IEnumerable<Atendimento>> GetByMedicoAsync(int medicoId);
}
