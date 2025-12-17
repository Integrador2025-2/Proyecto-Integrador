using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface ISeguimientoEvaluacionRepository
{
    Task<SeguimientoEvaluacion?> GetByIdAsync(int seguimientoId);
    Task<IEnumerable<SeguimientoEvaluacion>> GetAllAsync();
    Task<SeguimientoEvaluacion?> GetByRecursoEspecificoIdAsync(int recursoEspecificoId);
    Task<SeguimientoEvaluacion> CreateAsync(SeguimientoEvaluacion seguimientoEvaluacion);
    Task<SeguimientoEvaluacion> UpdateAsync(SeguimientoEvaluacion seguimientoEvaluacion);
    Task<bool> DeleteAsync(int seguimientoId);
    Task<bool> ExistsAsync(int seguimientoId);
}
