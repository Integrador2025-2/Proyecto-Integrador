using Backend.Models.Domain;
namespace Backend.Infrastructure.Repositories;

public interface IActividadRepository
{
    Task<List<Actividad>> GetAllAsync();
    Task<Actividad?> GetByIdAsync(int id);
    Task<List<Actividad>> GetByProyectoIdAsync(int proyectoId);
    Task<Actividad> CreateAsync(Actividad actividad);
    Task<Actividad?> UpdateAsync(Actividad actividad);
    Task<bool> DeleteAsync(int id);
}
