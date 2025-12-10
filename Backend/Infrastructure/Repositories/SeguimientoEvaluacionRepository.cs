using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class SeguimientoEvaluacionRepository : ISeguimientoEvaluacionRepository
{
    private readonly ApplicationDbContext _context;

    public SeguimientoEvaluacionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<SeguimientoEvaluacion?> GetByIdAsync(int seguimientoId)
    {
        return await _context.SeguimientoEvaluacion
            .Include(se => se.RecursoEspecifico)
            .FirstOrDefaultAsync(se => se.SeguimientoId == seguimientoId);
    }

    public async Task<IEnumerable<SeguimientoEvaluacion>> GetAllAsync()
    {
        return await _context.SeguimientoEvaluacion
            .Include(se => se.RecursoEspecifico)
            .ToListAsync();
    }

    public async Task<SeguimientoEvaluacion?> GetByRecursoEspecificoIdAsync(int recursoEspecificoId)
    {
        return await _context.SeguimientoEvaluacion
            .Include(se => se.RecursoEspecifico)
            .FirstOrDefaultAsync(se => se.RecursoEspecificoId == recursoEspecificoId);
    }

    public async Task<SeguimientoEvaluacion> CreateAsync(SeguimientoEvaluacion seguimientoEvaluacion)
    {
        await _context.SeguimientoEvaluacion.AddAsync(seguimientoEvaluacion);
        await _context.SaveChangesAsync();
        return seguimientoEvaluacion;
    }

    public async Task<SeguimientoEvaluacion> UpdateAsync(SeguimientoEvaluacion seguimientoEvaluacion)
    {
        _context.SeguimientoEvaluacion.Update(seguimientoEvaluacion);
        await _context.SaveChangesAsync();
        return seguimientoEvaluacion;
    }

    public async Task<bool> DeleteAsync(int seguimientoId)
    {
        var seguimientoEvaluacion = await _context.SeguimientoEvaluacion
            .FirstOrDefaultAsync(se => se.SeguimientoId == seguimientoId);
        
        if (seguimientoEvaluacion == null)
        {
            return false;
        }

        _context.SeguimientoEvaluacion.Remove(seguimientoEvaluacion);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int seguimientoId)
    {
        return await _context.SeguimientoEvaluacion
            .AnyAsync(se => se.SeguimientoId == seguimientoId);
    }
}
