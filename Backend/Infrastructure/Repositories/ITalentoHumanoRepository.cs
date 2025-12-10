using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface ITalentoHumanoRepository
{
    Task<TalentoHumano?> GetByIdAsync(int talentoHumanoId);
    Task<IEnumerable<TalentoHumano>> GetAllAsync();
    Task<IEnumerable<TalentoHumano>> GetByRecursoEspecificoIdAsync(int recursoEspecificoId);
    Task<TalentoHumano> CreateAsync(TalentoHumano talentoHumano);
    Task<TalentoHumano> UpdateAsync(TalentoHumano talentoHumano);
    Task DeleteAsync(int talentoHumanoId);
    Task<bool> ExistsAsync(int talentoHumanoId);
}
