using Backend.Models.Domain;
namespace Backend.Infrastructure.Repositories;

public interface IMaterialesInsumosRepository
{
    Task<List<MaterialesInsumos>> GetAllAsync();
    Task<MaterialesInsumos?> GetByIdAsync(int id);
    Task<List<MaterialesInsumos>> GetByRubroIdAsync(int rubroId);
    Task<MaterialesInsumos> CreateAsync(MaterialesInsumos entity);
    Task<MaterialesInsumos?> UpdateAsync(MaterialesInsumos entity);
    Task<bool> DeleteAsync(int id);
}
