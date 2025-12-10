using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class ActXEntidadRepository : IActXEntidadRepository
{
    private readonly ApplicationDbContext _context;

    public ActXEntidadRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ActXEntidad?> GetByIdAsync(int id)
    {
        return await _context.ActXEntidades
            .Include(a => a.Actividad)
            .Include(a => a.Entidad)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<ActXEntidad>> GetAllAsync()
    {
        return await _context.ActXEntidades
            .Include(a => a.Actividad)
            .Include(a => a.Entidad)
            .ToListAsync();
    }

    public async Task<IEnumerable<ActXEntidad>> GetByActividadIdAsync(int actividadId)
    {
        return await _context.ActXEntidades
            .Include(a => a.Actividad)
            .Include(a => a.Entidad)
            .Where(a => a.ActividadId == actividadId)
            .ToListAsync();
    }

    public async Task<IEnumerable<ActXEntidad>> GetByEntidadIdAsync(int entidadId)
    {
        return await _context.ActXEntidades
            .Include(a => a.Actividad)
            .Include(a => a.Entidad)
            .Where(a => a.EntidadId == entidadId)
            .ToListAsync();
    }

    public async Task<ActXEntidad> CreateAsync(ActXEntidad actXEntidad)
    {
        _context.ActXEntidades.Add(actXEntidad);
        await _context.SaveChangesAsync();
        return actXEntidad;
    }

    public async Task<ActXEntidad> UpdateAsync(ActXEntidad actXEntidad)
    {
        _context.ActXEntidades.Update(actXEntidad);
        await _context.SaveChangesAsync();
        return actXEntidad;
    }

    public async Task DeleteAsync(int id)
    {
        var actXEntidad = await GetByIdAsync(id);
        if (actXEntidad != null)
        {
            _context.ActXEntidades.Remove(actXEntidad);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.ActXEntidades.AnyAsync(a => a.Id == id);
    }
}
