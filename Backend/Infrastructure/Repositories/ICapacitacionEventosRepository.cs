using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface ICapacitacionEventosRepository
{
    Task<CapacitacionEventos?> GetByIdAsync(int capacitacionEventosId);
    Task<IEnumerable<CapacitacionEventos>> GetAllAsync();
    Task<CapacitacionEventos?> GetByRecursoEspecificoIdAsync(int recursoEspecificoId);
    Task<CapacitacionEventos> CreateAsync(CapacitacionEventos capacitacionEventos);
    Task<CapacitacionEventos> UpdateAsync(CapacitacionEventos capacitacionEventos);
    Task<bool> DeleteAsync(int capacitacionEventosId);
    Task<bool> ExistsAsync(int capacitacionEventosId);
}
