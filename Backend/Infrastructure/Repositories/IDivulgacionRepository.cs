using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IDivulgacionRepository
{
    Task<Divulgacion?> GetByIdAsync(int divulgacionId);
    Task<IEnumerable<Divulgacion>> GetAllAsync();
    Task<Divulgacion?> GetByRecursoEspecificoIdAsync(int recursoEspecificoId);
    Task<Divulgacion> CreateAsync(Divulgacion divulgacion);
    Task<Divulgacion> UpdateAsync(Divulgacion divulgacion);
    Task<bool> DeleteAsync(int divulgacionId);
    Task<bool> ExistsAsync(int divulgacionId);
}
