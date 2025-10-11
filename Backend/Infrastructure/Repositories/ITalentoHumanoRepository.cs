using Backend.Models.Domain;
namespace Backend.Infrastructure.Repositories;

public interface ITalentoHumanoRepository
{
    Task<List<TalentoHumano>> GetAllAsync();
    Task<TalentoHumano?> GetByIdAsync(int id);
    Task<List<TalentoHumano>> GetByRubroIdAsync(int rubroId);
    Task<TalentoHumano> CreateAsync(TalentoHumano entity);
    Task<TalentoHumano?> UpdateAsync(TalentoHumano entity);
    Task<bool> DeleteAsync(int id);
}
