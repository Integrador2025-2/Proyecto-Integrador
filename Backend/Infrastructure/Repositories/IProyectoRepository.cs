using Backend.Models.Domain;
namespace Backend.Infrastructure.Repositories;

public interface IProyectoRepository
{
    Task<List<Proyecto>> GetAllAsync();
    Task<Proyecto?> GetByIdAsync(int id);
    Task<Proyecto> CreateAsync(Proyecto proyecto);
}
