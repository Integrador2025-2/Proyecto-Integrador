using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface ICadenaDeValorRepository
{
    Task<CadenaDeValor?> GetByIdAsync(int id);
    Task<IEnumerable<CadenaDeValor>> GetAllAsync();
    Task<IEnumerable<CadenaDeValor>> GetByObjetivoIdAsync(int objetivoId);
    Task<CadenaDeValor> CreateAsync(CadenaDeValor cadenaDeValor);
    Task<CadenaDeValor> UpdateAsync(CadenaDeValor cadenaDeValor);
    Task DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
}
