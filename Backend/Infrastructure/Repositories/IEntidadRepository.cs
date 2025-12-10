using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IEntidadRepository
{
    Task<List<Entidad>> GetAllAsync();
    Task<Entidad?> GetByIdAsync(int id);
    Task<List<Entidad>> GetByFilterAsync(string? searchTerm = null);
    Task<Entidad> CreateAsync(Entidad entidad);
    Task<Entidad> UpdateAsync(Entidad entidad);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
