using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IProyectoRepository
{
    Task<Proyecto?> GetByIdAsync(int id);
    Task<IEnumerable<Proyecto>> GetAllAsync();
    Task<IEnumerable<Proyecto>> GetByUsuarioIdAsync(int usuarioId);
    Task<Proyecto> CreateAsync(Proyecto proyecto);
    Task<Proyecto> UpdateAsync(Proyecto proyecto);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
