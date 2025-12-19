using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IContratacionRepository
{
    Task<Contratacion?> GetByIdAsync(int contratacionId);
    Task<IEnumerable<Contratacion>> GetAllAsync();
    Task<IEnumerable<Contratacion>> GetByCategoriaAsync(string categoria);
    Task<Contratacion> CreateAsync(Contratacion contratacion);
    Task<Contratacion> UpdateAsync(Contratacion contratacion);
    Task DeleteAsync(int contratacionId);
    Task<bool> ExistsAsync(int contratacionId);
}
