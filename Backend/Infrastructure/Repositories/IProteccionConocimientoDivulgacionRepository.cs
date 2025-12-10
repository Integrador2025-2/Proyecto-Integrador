using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface IProteccionConocimientoDivulgacionRepository
{
    Task<ProteccionConocimientoDivulgacion?> GetByIdAsync(int proteccionId);
    Task<IEnumerable<ProteccionConocimientoDivulgacion>> GetAllAsync();
    Task<ProteccionConocimientoDivulgacion?> GetByRecursoEspecificoIdAsync(int recursoEspecificoId);
    Task<ProteccionConocimientoDivulgacion> CreateAsync(ProteccionConocimientoDivulgacion proteccionConocimientoDivulgacion);
    Task<ProteccionConocimientoDivulgacion> UpdateAsync(ProteccionConocimientoDivulgacion proteccionConocimientoDivulgacion);
    Task<bool> DeleteAsync(int proteccionId);
    Task<bool> ExistsAsync(int proteccionId);
}
