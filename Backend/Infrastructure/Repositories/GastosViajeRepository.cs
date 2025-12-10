using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class GastosViajeRepository : IGastosViajeRepository
{
    private readonly ApplicationDbContext _context;

    public GastosViajeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GastosViaje?> GetByIdAsync(int id)
    {
        return await _context.GastosViaje
            .Include(g => g.RecursoEspecifico)
            .FirstOrDefaultAsync(g => g.GastosViajeId == id);
    }

    public async Task<IEnumerable<GastosViaje>> GetAllAsync()
    {
        return await _context.GastosViaje
            .Include(g => g.RecursoEspecifico)
            .ToListAsync();
    }

    public async Task<GastosViaje?> GetByRecursoEspecificoIdAsync(int recursoEspecificoId)
    {
        return await _context.GastosViaje
            .Include(g => g.RecursoEspecifico)
            .FirstOrDefaultAsync(g => g.RecursoEspecificoId == recursoEspecificoId);
    }

    public async Task<GastosViaje> CreateAsync(GastosViaje gastosViaje)
    {
        _context.GastosViaje.Add(gastosViaje);
        await _context.SaveChangesAsync();
        return gastosViaje;
    }

    public async Task<GastosViaje> UpdateAsync(GastosViaje gastosViaje)
    {
        _context.GastosViaje.Update(gastosViaje);
        await _context.SaveChangesAsync();
        return gastosViaje;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var gastosViaje = await _context.GastosViaje.FindAsync(id);
        if (gastosViaje == null)
            return false;

        _context.GastosViaje.Remove(gastosViaje);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.GastosViaje.AnyAsync(g => g.GastosViajeId == id);
    }
}
