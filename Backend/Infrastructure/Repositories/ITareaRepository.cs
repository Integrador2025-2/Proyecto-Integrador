using Backend.Models.Domain;
namespace Backend.Infrastructure.Repositories;

public interface ITareaRepository
{
    Task<List<Tarea>> GetAllAsync(int? actividadId = null);
    Task<Tarea?> GetByIdAsync(int id);
    Task<Tarea> CreateAsync(Tarea entity);
    Task<Tarea> UpdateAsync(Tarea entity);
    Task<bool> DeleteAsync(int id);
}
