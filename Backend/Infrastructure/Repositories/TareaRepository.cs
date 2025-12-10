using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class TareaRepository : ITareaRepository
{
    private readonly ApplicationDbContext _context;

    public TareaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Tarea?> GetByIdAsync(int id)
    {
        return await _context.Tareas
            .Include(t => t.Actividad)
            .Include(t => t.CronogramaTareas)
            .Include(t => t.TalentoHumanoTareas)
            .FirstOrDefaultAsync(t => t.TareaId == id);
    }

    public async Task<IEnumerable<Tarea>> GetAllAsync()
    {
        return await _context.Tareas
            .Include(t => t.Actividad)
            .Include(t => t.CronogramaTareas)
            .Include(t => t.TalentoHumanoTareas)
            .ToListAsync();
    }

    public async Task<IEnumerable<Tarea>> GetByActividadIdAsync(int actividadId)
    {
        return await _context.Tareas
            .Include(t => t.Actividad)
            .Include(t => t.CronogramaTareas)
            .Include(t => t.TalentoHumanoTareas)
            .Where(t => t.ActividadId == actividadId)
            .ToListAsync();
    }

    public async Task<Tarea> CreateAsync(Tarea tarea)
    {
        await _context.Tareas.AddAsync(tarea);
        await _context.SaveChangesAsync();
        return tarea;
    }

    public async Task<Tarea> UpdateAsync(Tarea tarea)
    {
        _context.Tareas.Update(tarea);
        await _context.SaveChangesAsync();
        return tarea;
    }

    public async Task DeleteAsync(int id)
    {
        var tarea = await _context.Tareas.FindAsync(id);
        if (tarea != null)
        {
            _context.Tareas.Remove(tarea);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Tareas.AnyAsync(t => t.TareaId == id);
    }
}
