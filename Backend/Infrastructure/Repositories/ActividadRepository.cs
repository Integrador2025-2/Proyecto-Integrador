using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class ActividadRepository : IActividadRepository
{
    private readonly ApplicationDbContext _context;

    public ActividadRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Actividad?> GetByIdAsync(int id)
    {
        return await _context.Actividades
            .Include(a => a.CadenaDeValor)
            .Include(a => a.Tareas)
            .Include(a => a.Recursos)
            .Include(a => a.ActXEntidades)
            .FirstOrDefaultAsync(a => a.ActividadId == id);
    }

    public async Task<IEnumerable<Actividad>> GetAllAsync()
    {
        return await _context.Actividades
            .Include(a => a.CadenaDeValor)
            .Include(a => a.Tareas)
            .Include(a => a.Recursos)
            .Include(a => a.ActXEntidades)
            .ToListAsync();
    }

    public async Task<IEnumerable<Actividad>> GetByCadenaDeValorIdAsync(int cadenaDeValorId)
    {
        return await _context.Actividades
            .Include(a => a.CadenaDeValor)
            .Include(a => a.Tareas)
            .Include(a => a.Recursos)
            .Include(a => a.ActXEntidades)
            .Where(a => a.CadenaDeValorId == cadenaDeValorId)
            .ToListAsync();
    }

    public async Task<Actividad> CreateAsync(Actividad actividad)
    {
        await _context.Actividades.AddAsync(actividad);
        await _context.SaveChangesAsync();
        return actividad;
    }

    public async Task<Actividad> UpdateAsync(Actividad actividad)
    {
        _context.Actividades.Update(actividad);
        await _context.SaveChangesAsync();
        return actividad;
    }

    public async Task DeleteAsync(int id)
    {
        var actividad = await _context.Actividades.FindAsync(id);
        if (actividad != null)
        {
            _context.Actividades.Remove(actividad);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Actividades.AnyAsync(a => a.ActividadId == id);
    }
}
