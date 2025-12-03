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
            .Include(ae => ae.Entidad)
            .Include(ae => ae.Actividad)
            .FirstOrDefaultAsync(ae => ae.ActXEntidadId == id);
    }

    public async Task<List<ActXEntidad>> GetAllAsync(int? actividadId = null)
    {
        var q = _context.ActXEntidades
            .Include(ae => ae.Entidad)
            .Include(ae => ae.Actividad)
            .AsQueryable();

        if (actividadId.HasValue)
            q = q.Where(ae => ae.ActividadId == actividadId.Value);

        return await q.ToListAsync();
    }

    public async Task<ActXEntidad> CreateAsync(ActXEntidad actxEntidad)
    {
        _context.ActXEntidades.Add(actxEntidad);
        await _context.SaveChangesAsync();
        return actxEntidad;
    }

    public async Task<ActXEntidad> UpdateAsync(ActXEntidad actxEntidad)
    {
        _context.ActXEntidades.Update(actxEntidad);
        await _context.SaveChangesAsync();
        return actxEntidad;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _context.ActXEntidades.FindAsync(id);
        if (e == null) return false;
        _context.ActXEntidades.Remove(e);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.ActXEntidades.AnyAsync(e => e.ActXEntidadId == id);
    }
}
