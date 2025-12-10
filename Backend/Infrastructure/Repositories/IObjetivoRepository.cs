using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IObjetivoRepository
{
    Task<Objetivo?> GetByIdAsync(int id);
    Task<IEnumerable<Objetivo>> GetAllAsync();
    Task<IEnumerable<Objetivo>> GetByProyectoIdAsync(int proyectoId);
    Task<Objetivo> CreateAsync(Objetivo objetivo);
    Task<Objetivo> UpdateAsync(Objetivo objetivo);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
