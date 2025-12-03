using Backend.Models.Domain;
namespace Backend.Infrastructure.Repositories;

public interface IGastosViajeRepository
{
    Task<List<GastosViaje>> GetAllAsync();
    Task<GastosViaje?> GetByIdAsync(int id);
    Task<List<GastosViaje>> GetByRubroIdAsync(int rubroId);
    Task<GastosViaje> CreateAsync(GastosViaje entity);
    Task<GastosViaje?> UpdateAsync(GastosViaje entity);
    Task<bool> DeleteAsync(int id);
}
