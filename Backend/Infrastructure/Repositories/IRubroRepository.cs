using Backend.Models.Domain;
namespace Backend.Infrastructure.Repositories;

public interface IRubroRepository
{
    Task<List<Rubro>> GetAllAsync();
    Task<Rubro?> GetByIdAsync(int id);
    Task<Rubro> CreateAsync(Rubro rubro);
    Task<Rubro?> UpdateAsync(Rubro rubro);
    Task<bool> DeleteAsync(int id);
}
