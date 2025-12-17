using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IGastosViajeRepository
{
    Task<GastosViaje?> GetByIdAsync(int id);
    Task<IEnumerable<GastosViaje>> GetAllAsync();
    Task<GastosViaje?> GetByRecursoEspecificoIdAsync(int recursoEspecificoId);
    Task<GastosViaje> CreateAsync(GastosViaje gastosViaje);
    Task<GastosViaje> UpdateAsync(GastosViaje gastosViaje);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
