using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class ProteccionConocimientoDivulgacionRepository : IProteccionConocimientoDivulgacionRepository
{
    private readonly ApplicationDbContext _context;

    public ProteccionConocimientoDivulgacionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ProteccionConocimientoDivulgacion?> GetByIdAsync(int proteccionId)
    {
        return await _context.ProteccionConocimientoDivulgacion
            .Include(pcd => pcd.RecursoEspecifico)
            .FirstOrDefaultAsync(pcd => pcd.ProteccionId == proteccionId);
    }

    public async Task<IEnumerable<ProteccionConocimientoDivulgacion>> GetAllAsync()
    {
        return await _context.ProteccionConocimientoDivulgacion
            .Include(pcd => pcd.RecursoEspecifico)
            .ToListAsync();
    }

    public async Task<ProteccionConocimientoDivulgacion?> GetByRecursoEspecificoIdAsync(int recursoEspecificoId)
    {
        return await _context.ProteccionConocimientoDivulgacion
            .Include(pcd => pcd.RecursoEspecifico)
            .FirstOrDefaultAsync(pcd => pcd.RecursoEspecificoId == recursoEspecificoId);
    }

    public async Task<ProteccionConocimientoDivulgacion> CreateAsync(ProteccionConocimientoDivulgacion proteccionConocimientoDivulgacion)
    {
        await _context.ProteccionConocimientoDivulgacion.AddAsync(proteccionConocimientoDivulgacion);
        await _context.SaveChangesAsync();
        return proteccionConocimientoDivulgacion;
    }

    public async Task<ProteccionConocimientoDivulgacion> UpdateAsync(ProteccionConocimientoDivulgacion proteccionConocimientoDivulgacion)
    {
        _context.ProteccionConocimientoDivulgacion.Update(proteccionConocimientoDivulgacion);
        await _context.SaveChangesAsync();
        return proteccionConocimientoDivulgacion;
    }

    public async Task<bool> DeleteAsync(int proteccionId)
    {
        var proteccionConocimientoDivulgacion = await _context.ProteccionConocimientoDivulgacion
            .FirstOrDefaultAsync(pcd => pcd.ProteccionId == proteccionId);
        
        if (proteccionConocimientoDivulgacion == null)
        {
            return false;
        }

        _context.ProteccionConocimientoDivulgacion.Remove(proteccionConocimientoDivulgacion);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int proteccionId)
    {
        return await _context.ProteccionConocimientoDivulgacion
            .AnyAsync(pcd => pcd.ProteccionId == proteccionId);
    }
}
