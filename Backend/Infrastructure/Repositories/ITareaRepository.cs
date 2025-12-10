using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface ITareaRepository
{
    Task<Tarea?> GetByIdAsync(int id);
    Task<IEnumerable<Tarea>> GetAllAsync();
    Task<IEnumerable<Tarea>> GetByActividadIdAsync(int actividadId);
    Task<Tarea> CreateAsync(Tarea tarea);
    Task<Tarea> UpdateAsync(Tarea tarea);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
