using Backend.Models.Domain;
namespace Backend.Infrastructure.Repositories;

public interface IEquiposSoftwareRepository
{
    Task<List<EquiposSoftware>> GetAllAsync();
    Task<EquiposSoftware?> GetByIdAsync(int id);
    Task<List<EquiposSoftware>> GetByRubroIdAsync(int rubroId);
    Task<EquiposSoftware> CreateAsync(EquiposSoftware entity);
    Task<EquiposSoftware?> UpdateAsync(EquiposSoftware entity);
    Task<bool> DeleteAsync(int id);
}
