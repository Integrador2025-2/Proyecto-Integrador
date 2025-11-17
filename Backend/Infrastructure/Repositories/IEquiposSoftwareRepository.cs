using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IEquiposSoftwareRepository
{
    Task<EquiposSoftware?> GetByIdAsync(int equiposSoftwareId);
    Task<IEnumerable<EquiposSoftware>> GetAllAsync();
    Task<EquiposSoftware?> GetByRecursoEspecificoIdAsync(int recursoEspecificoId);
    Task<EquiposSoftware> CreateAsync(EquiposSoftware equiposSoftware);
    Task<EquiposSoftware> UpdateAsync(EquiposSoftware equiposSoftware);
    Task<bool> DeleteAsync(int equiposSoftwareId);
    Task<bool> ExistsAsync(int equiposSoftwareId);
}
