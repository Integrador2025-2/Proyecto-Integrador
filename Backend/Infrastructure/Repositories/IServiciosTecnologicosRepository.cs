using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IServiciosTecnologicosRepository
{
    Task<ServiciosTecnologicos?> GetByIdAsync(int serviciosTecnologicosId);
    Task<IEnumerable<ServiciosTecnologicos>> GetAllAsync();
    Task<ServiciosTecnologicos?> GetByRecursoEspecificoIdAsync(int recursoEspecificoId);
    Task<ServiciosTecnologicos> CreateAsync(ServiciosTecnologicos serviciosTecnologicos);
    Task<ServiciosTecnologicos> UpdateAsync(ServiciosTecnologicos serviciosTecnologicos);
    Task<bool> DeleteAsync(int serviciosTecnologicosId);
    Task<bool> ExistsAsync(int serviciosTecnologicosId);
}
