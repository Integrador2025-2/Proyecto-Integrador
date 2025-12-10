using Backend.Models.Domain;

namespace Backend.Infrastructure.Repositories;

public interface ICronogramaTareaRepository
{
    Task<CronogramaTarea?> GetByIdAsync(int cronogramaId);
    Task<IEnumerable<CronogramaTarea>> GetAllAsync();
    Task<IEnumerable<CronogramaTarea>> GetByTareaIdAsync(int tareaId);
    Task<CronogramaTarea> CreateAsync(CronogramaTarea cronogramaTarea);
    Task<CronogramaTarea> UpdateAsync(CronogramaTarea cronogramaTarea);
    Task DeleteAsync(int cronogramaId);
    Task<bool> ExistsAsync(int cronogramaId);
}
