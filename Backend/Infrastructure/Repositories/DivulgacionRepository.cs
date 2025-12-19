using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class DivulgacionRepository : IDivulgacionRepository
{
    private readonly ApplicationDbContext _context;

    public DivulgacionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Divulgacion?> GetByIdAsync(int divulgacionId)
    {
        return await _context.Divulgacion
            .Include(d => d.RecursoEspecifico)
            .FirstOrDefaultAsync(d => d.DivulgacionId == divulgacionId);
    }

    public async Task<IEnumerable<Divulgacion>> GetAllAsync()
    {
        return await _context.Divulgacion
            .Include(d => d.RecursoEspecifico)
            .ToListAsync();
    }

    public async Task<Divulgacion?> GetByRecursoEspecificoIdAsync(int recursoEspecificoId)
    {
        return await _context.Divulgacion
            .Include(d => d.RecursoEspecifico)
            .FirstOrDefaultAsync(d => d.RecursoEspecificoId == recursoEspecificoId);
    }

    public async Task<Divulgacion> CreateAsync(Divulgacion divulgacion)
    {
        await _context.Divulgacion.AddAsync(divulgacion);
        await _context.SaveChangesAsync();
        return divulgacion;
    }

    public async Task<Divulgacion> UpdateAsync(Divulgacion divulgacion)
    {
        _context.Divulgacion.Update(divulgacion);
        await _context.SaveChangesAsync();
        return divulgacion;
    }

    public async Task<bool> DeleteAsync(int divulgacionId)
    {
        var divulgacion = await _context.Divulgacion
            .FirstOrDefaultAsync(d => d.DivulgacionId == divulgacionId);
        
        if (divulgacion == null)
        {
            return false;
        }

        _context.Divulgacion.Remove(divulgacion);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int divulgacionId)
    {
        return await _context.Divulgacion
            .AnyAsync(d => d.DivulgacionId == divulgacionId);
    }
}
