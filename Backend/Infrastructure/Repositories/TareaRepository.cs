using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class TareaRepository : ITareaRepository
{
    private readonly ApplicationDbContext _context;
    public TareaRepository(ApplicationDbContext context) => _context = context;

    public async Task<List<Tarea>> GetAllAsync(int? actividadId = null)
    {
        var q = _context.Tareas.AsQueryable();
        if (actividadId.HasValue)
            q = q.Where(t => t.ActividadId == actividadId.Value);
        return await q.AsNoTracking().ToListAsync();
    }

    public async Task<Tarea?> GetByIdAsync(int id) => await _context.Tareas.FindAsync(id);

    public async Task<Tarea> CreateAsync(Tarea entity)
    {
        _context.Tareas.Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Tarea> UpdateAsync(Tarea entity)
    {
        _context.Tareas.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _context.Tareas.FindAsync(id);
        if (e == null) return false;
        _context.Tareas.Remove(e);
        await _context.SaveChangesAsync();
        return true;
    }
}
