using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IAdministrativosRepository
{
    Task<Administrativos?> GetByIdAsync(int administrativoId);
    Task<IEnumerable<Administrativos>> GetAllAsync();
    Task<Administrativos?> GetByRecursoEspecificoIdAsync(int recursoEspecificoId);
    Task<Administrativos> CreateAsync(Administrativos administrativos);
    Task<Administrativos> UpdateAsync(Administrativos administrativos);
    Task<bool> DeleteAsync(int administrativoId);
    Task<bool> ExistsAsync(int administrativoId);
}
