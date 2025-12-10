using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IRemuneracionPorAnioRepository
{
    Task<RemuneracionPorAnio?> GetByIdAsync(int remuneracionPorAnioId);
    Task<IEnumerable<RemuneracionPorAnio>> GetAllAsync();
    Task<IEnumerable<RemuneracionPorAnio>> GetByTalentoHumanoIdAsync(int talentoHumanoId);
    Task<IEnumerable<RemuneracionPorAnio>> GetByAnioAsync(int anio);
    Task<RemuneracionPorAnio> CreateAsync(RemuneracionPorAnio remuneracionPorAnio);
    Task<RemuneracionPorAnio> UpdateAsync(RemuneracionPorAnio remuneracionPorAnio);
    Task DeleteAsync(int remuneracionPorAnioId);
    Task<bool> ExistsAsync(int remuneracionPorAnioId);
}
