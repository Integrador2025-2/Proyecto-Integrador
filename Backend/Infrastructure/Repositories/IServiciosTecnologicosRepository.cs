using Backend.Models.Domain;
namespace Backend.Infrastructure.Repositories;

public interface IServiciosTecnologicosRepository
{
    Task<List<ServiciosTecnologicos>> GetAllAsync();
    Task<ServiciosTecnologicos?> GetByIdAsync(int id);
    Task<List<ServiciosTecnologicos>> GetByRubroIdAsync(int rubroId);
    Task<ServiciosTecnologicos> CreateAsync(ServiciosTecnologicos entity);
    Task<ServiciosTecnologicos?> UpdateAsync(ServiciosTecnologicos entity);
    Task<bool> DeleteAsync(int id);
}
