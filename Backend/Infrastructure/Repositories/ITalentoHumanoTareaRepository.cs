using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface ITalentoHumanoTareaRepository
{
    Task<TalentoHumanoTarea?> GetByIdAsync(int talentoHumanoTareasId);
    Task<IEnumerable<TalentoHumanoTarea>> GetAllAsync();
    Task<IEnumerable<TalentoHumanoTarea>> GetByTalentoHumanoIdAsync(int talentoHumanoId);
    Task<IEnumerable<TalentoHumanoTarea>> GetByTareaIdAsync(int tareaId);
    Task<TalentoHumanoTarea> CreateAsync(TalentoHumanoTarea talentoHumanoTarea);
    Task<TalentoHumanoTarea> UpdateAsync(TalentoHumanoTarea talentoHumanoTarea);
    Task DeleteAsync(int talentoHumanoTareasId);
    Task<bool> ExistsAsync(int talentoHumanoTareasId);
}
