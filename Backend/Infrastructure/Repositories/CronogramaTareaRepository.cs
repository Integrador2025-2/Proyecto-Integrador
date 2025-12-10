using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class CronogramaTareaRepository : ICronogramaTareaRepository
{
    private readonly ApplicationDbContext _context;

    public CronogramaTareaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CronogramaTarea?> GetByIdAsync(int cronogramaId)
    {
        return await _context.CronogramaTareas
            .Include(c => c.Tarea)
            .FirstOrDefaultAsync(c => c.CronogramaId == cronogramaId);
    }

    public async Task<IEnumerable<CronogramaTarea>> GetAllAsync()
    {
        return await _context.CronogramaTareas
            .Include(c => c.Tarea)
            .ToListAsync();
    }

    public async Task<IEnumerable<CronogramaTarea>> GetByTareaIdAsync(int tareaId)
    {
        return await _context.CronogramaTareas
            .Include(c => c.Tarea)
            .Where(c => c.TareaId == tareaId)
            .ToListAsync();
    }

    public async Task<CronogramaTarea> CreateAsync(CronogramaTarea cronogramaTarea)
    {
        _context.CronogramaTareas.Add(cronogramaTarea);
        await _context.SaveChangesAsync();
        return cronogramaTarea;
    }

    public async Task<CronogramaTarea> UpdateAsync(CronogramaTarea cronogramaTarea)
    {
        _context.CronogramaTareas.Update(cronogramaTarea);
        await _context.SaveChangesAsync();
        return cronogramaTarea;
    }

    public async Task DeleteAsync(int cronogramaId)
    {
        var cronogramaTarea = await GetByIdAsync(cronogramaId);
        if (cronogramaTarea != null)
        {
            _context.CronogramaTareas.Remove(cronogramaTarea);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int cronogramaId)
    {
        return await _context.CronogramaTareas.AnyAsync(c => c.CronogramaId == cronogramaId);
    }
}
