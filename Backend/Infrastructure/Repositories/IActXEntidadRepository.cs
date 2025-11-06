using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IActXEntidadRepository
{
    Task<ActXEntidad?> GetByIdAsync(int id);
    Task<IEnumerable<ActXEntidad>> GetAllAsync();
    Task<IEnumerable<ActXEntidad>> GetByActividadIdAsync(int actividadId);
    Task<IEnumerable<ActXEntidad>> GetByEntidadIdAsync(int entidadId);
    Task<ActXEntidad> CreateAsync(ActXEntidad actXEntidad);
    Task<ActXEntidad> UpdateAsync(ActXEntidad actXEntidad);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
