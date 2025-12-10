using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IRubroRepository
{
    Task<Rubro?> GetByIdAsync(int id);
    Task<IEnumerable<Rubro>> GetAllAsync();
    Task<Rubro> CreateAsync(Rubro rubro);
    Task<Rubro> UpdateAsync(Rubro rubro);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
