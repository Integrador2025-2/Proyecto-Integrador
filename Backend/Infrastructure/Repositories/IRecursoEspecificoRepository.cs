using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IRecursoEspecificoRepository
{
    Task<RecursoEspecifico?> GetByIdAsync(int id);
    Task<IEnumerable<RecursoEspecifico>> GetAllAsync();
    Task<IEnumerable<RecursoEspecifico>> GetByRecursoIdAsync(int recursoId);
    Task<IEnumerable<RecursoEspecifico>> GetByTipoAsync(string tipo);
    Task<RecursoEspecifico> CreateAsync(RecursoEspecifico recursoEspecifico);
    Task<RecursoEspecifico> UpdateAsync(RecursoEspecifico recursoEspecifico);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
