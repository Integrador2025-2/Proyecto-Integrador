using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IRecursoRepository
{
    Task<Recurso?> GetByIdAsync(int id);
    Task<IEnumerable<Recurso>> GetAllAsync();
    Task<IEnumerable<Recurso>> GetByActividadIdAsync(int actividadId);
    Task<Recurso> CreateAsync(Recurso recurso);
    Task<Recurso> UpdateAsync(Recurso recurso);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
