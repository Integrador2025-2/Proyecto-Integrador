using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class RecursoRepository : IRecursoRepository
{
    private readonly ApplicationDbContext _context;

    public RecursoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Recurso?> GetByIdAsync(int id)
    {
        return await _context.Recursos
            .Include(r => r.Actividad)
            .Include(r => r.RecursosEspecificos)
            .FirstOrDefaultAsync(r => r.RecursoId == id);
    }

    public async Task<IEnumerable<Recurso>> GetAllAsync()
    {
        return await _context.Recursos
            .Include(r => r.Actividad)
            .Include(r => r.RecursosEspecificos)
            .ToListAsync();
    }

    public async Task<IEnumerable<Recurso>> GetByActividadIdAsync(int actividadId)
    {
        return await _context.Recursos
            .Include(r => r.Actividad)
            .Include(r => r.RecursosEspecificos)
            .Where(r => r.ActividadId == actividadId)
            .ToListAsync();
    }

    public async Task<Recurso> CreateAsync(Recurso recurso)
    {
        await _context.Recursos.AddAsync(recurso);
        await _context.SaveChangesAsync();
        return recurso;
    }

    public async Task<Recurso> UpdateAsync(Recurso recurso)
    {
        _context.Recursos.Update(recurso);
        await _context.SaveChangesAsync();
        return recurso;
    }

    public async Task DeleteAsync(int id)
    {
        var recurso = await _context.Recursos.FindAsync(id);
        if (recurso != null)
        {
            _context.Recursos.Remove(recurso);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Recursos.AnyAsync(r => r.RecursoId == id);
    }
}
