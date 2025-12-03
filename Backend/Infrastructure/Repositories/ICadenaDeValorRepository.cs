using Backend.Models.Domain;
namespace Backend.Infrastructure.Repositories;

public interface ICadenaDeValorRepository
{
    Task<List<CadenaDeValor>> GetAllAsync();
    Task<CadenaDeValor?> GetByIdAsync(int id);
    Task<CadenaDeValor> CreateAsync(CadenaDeValor entity);
    Task<CadenaDeValor> UpdateAsync(CadenaDeValor entity);
    Task<bool> DeleteAsync(int id);
}
