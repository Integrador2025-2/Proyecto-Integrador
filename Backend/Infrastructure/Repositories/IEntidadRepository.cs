using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IEntidadRepository
{
    Task<Entidad?> GetByIdAsync(int id);
    Task<IEnumerable<Entidad>> GetAllAsync();
    Task<Entidad> CreateAsync(Entidad entidad);
    Task<Entidad> UpdateAsync(Entidad entidad);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
