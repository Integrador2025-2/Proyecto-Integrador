using Backend.Models.Domain;
namespace Backend.Infrastructure.Repositories;

public interface ICapacitacionEventosRepository
{
    Task<List<CapacitacionEventos>> GetAllAsync();
    Task<CapacitacionEventos?> GetByIdAsync(int id);
    Task<List<CapacitacionEventos>> GetByRubroIdAsync(int rubroId);
    Task<CapacitacionEventos> CreateAsync(CapacitacionEventos entity);
    Task<CapacitacionEventos?> UpdateAsync(CapacitacionEventos entity);
    Task<bool> DeleteAsync(int id);
}
