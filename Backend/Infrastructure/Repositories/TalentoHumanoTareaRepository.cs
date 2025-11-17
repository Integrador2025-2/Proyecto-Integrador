using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class TalentoHumanoTareaRepository : ITalentoHumanoTareaRepository
{
    private readonly ApplicationDbContext _context;

    public TalentoHumanoTareaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TalentoHumanoTarea?> GetByIdAsync(int talentoHumanoTareasId)
    {
        return await _context.TalentoHumanoTareas
            .Include(tht => tht.TalentoHumano)
            .Include(tht => tht.TareaNavigation)
            .FirstOrDefaultAsync(tht => tht.TalentoHumanoTareasId == talentoHumanoTareasId);
    }

    public async Task<IEnumerable<TalentoHumanoTarea>> GetAllAsync()
    {
        return await _context.TalentoHumanoTareas
            .Include(tht => tht.TalentoHumano)
            .Include(tht => tht.TareaNavigation)
            .ToListAsync();
    }

    public async Task<IEnumerable<TalentoHumanoTarea>> GetByTalentoHumanoIdAsync(int talentoHumanoId)
    {
        return await _context.TalentoHumanoTareas
            .Include(tht => tht.TalentoHumano)
            .Include(tht => tht.TareaNavigation)
            .Where(tht => tht.TalentoHumanoId == talentoHumanoId)
            .ToListAsync();
    }

    public async Task<IEnumerable<TalentoHumanoTarea>> GetByTareaIdAsync(int tareaId)
    {
        return await _context.TalentoHumanoTareas
            .Include(tht => tht.TalentoHumano)
            .Include(tht => tht.TareaNavigation)
            .Where(tht => tht.Tarea == tareaId)
            .ToListAsync();
    }

    public async Task<TalentoHumanoTarea> CreateAsync(TalentoHumanoTarea talentoHumanoTarea)
    {
        await _context.TalentoHumanoTareas.AddAsync(talentoHumanoTarea);
        await _context.SaveChangesAsync();
        return talentoHumanoTarea;
    }

    public async Task<TalentoHumanoTarea> UpdateAsync(TalentoHumanoTarea talentoHumanoTarea)
    {
        _context.TalentoHumanoTareas.Update(talentoHumanoTarea);
        await _context.SaveChangesAsync();
        return talentoHumanoTarea;
    }

    public async Task DeleteAsync(int talentoHumanoTareasId)
    {
        var talentoHumanoTarea = await _context.TalentoHumanoTareas.FindAsync(talentoHumanoTareasId);
        if (talentoHumanoTarea != null)
        {
            _context.TalentoHumanoTareas.Remove(talentoHumanoTarea);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int talentoHumanoTareasId)
    {
        return await _context.TalentoHumanoTareas.AnyAsync(tht => tht.TalentoHumanoTareasId == talentoHumanoTareasId);
    }
}
