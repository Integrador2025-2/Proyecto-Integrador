using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IMaterialesInsumosRepository
{
    Task<MaterialesInsumos?> GetByIdAsync(int materialesInsumosId);
    Task<IEnumerable<MaterialesInsumos>> GetAllAsync();
    Task<MaterialesInsumos?> GetByRecursoEspecificoIdAsync(int recursoEspecificoId);
    Task<MaterialesInsumos> CreateAsync(MaterialesInsumos materialesInsumos);
    Task<MaterialesInsumos> UpdateAsync(MaterialesInsumos materialesInsumos);
    Task<bool> DeleteAsync(int materialesInsumosId);
    Task<bool> ExistsAsync(int materialesInsumosId);
}
