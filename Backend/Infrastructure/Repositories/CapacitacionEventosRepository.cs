using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class CapacitacionEventosRepository : ICapacitacionEventosRepository
{
    private readonly ApplicationDbContext _context;

    public CapacitacionEventosRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CapacitacionEventos?> GetByIdAsync(int capacitacionEventosId)
    {
        return await _context.CapacitacionEventos
            .Include(ce => ce.RecursoEspecifico)
            .FirstOrDefaultAsync(ce => ce.CapacitacionEventosId == capacitacionEventosId);
    }

    public async Task<IEnumerable<CapacitacionEventos>> GetAllAsync()
    {
        return await _context.CapacitacionEventos
            .Include(ce => ce.RecursoEspecifico)
            .ToListAsync();
    }

    public async Task<CapacitacionEventos?> GetByRecursoEspecificoIdAsync(int recursoEspecificoId)
    {
        return await _context.CapacitacionEventos
            .Include(ce => ce.RecursoEspecifico)
            .FirstOrDefaultAsync(ce => ce.RecursoEspecificoId == recursoEspecificoId);
    }

    public async Task<CapacitacionEventos> CreateAsync(CapacitacionEventos capacitacionEventos)
    {
        await _context.CapacitacionEventos.AddAsync(capacitacionEventos);
        await _context.SaveChangesAsync();
        return capacitacionEventos;
    }

    public async Task<CapacitacionEventos> UpdateAsync(CapacitacionEventos capacitacionEventos)
    {
        _context.CapacitacionEventos.Update(capacitacionEventos);
        await _context.SaveChangesAsync();
        return capacitacionEventos;
    }

    public async Task<bool> DeleteAsync(int capacitacionEventosId)
    {
        var capacitacionEventos = await _context.CapacitacionEventos
            .FirstOrDefaultAsync(ce => ce.CapacitacionEventosId == capacitacionEventosId);
        
        if (capacitacionEventos == null)
        {
            return false;
        }

        _context.CapacitacionEventos.Remove(capacitacionEventos);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int capacitacionEventosId)
    {
        return await _context.CapacitacionEventos
            .AnyAsync(ce => ce.CapacitacionEventosId == capacitacionEventosId);
    }
}
