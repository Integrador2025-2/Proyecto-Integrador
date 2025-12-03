using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IActXEntidadRepository
{
    Task<List<ActXEntidad>> GetAllAsync(int? actividadId = null);
    Task<ActXEntidad?> GetByIdAsync(int id);
    Task<ActXEntidad> CreateAsync(ActXEntidad actxEntidad);
    Task<ActXEntidad> UpdateAsync(ActXEntidad actxEntidad);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
