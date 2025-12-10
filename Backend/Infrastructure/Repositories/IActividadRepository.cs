using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IActividadRepository
{
    Task<Actividad?> GetByIdAsync(int id);
    Task<IEnumerable<Actividad>> GetAllAsync();
    Task<IEnumerable<Actividad>> GetByCadenaDeValorIdAsync(int cadenaDeValorId);
    Task<Actividad> CreateAsync(Actividad actividad);
    Task<Actividad> UpdateAsync(Actividad actividad);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
