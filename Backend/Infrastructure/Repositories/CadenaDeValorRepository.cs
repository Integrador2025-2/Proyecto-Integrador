using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class CadenaDeValorRepository : ICadenaDeValorRepository
{
    private readonly ApplicationDbContext _context;

    public CadenaDeValorRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CadenaDeValor?> GetByIdAsync(int id)
    {
        return await _context.CadenasDeValor
            .Include(c => c.Objetivo)
            .Include(c => c.Actividades)
            .FirstOrDefaultAsync(c => c.CadenaDeValorId == id);
    }

    public async Task<IEnumerable<CadenaDeValor>> GetAllAsync()
    {
        return await _context.CadenasDeValor
            .Include(c => c.Objetivo)
            .Include(c => c.Actividades)
            .ToListAsync();
    }

    public async Task<IEnumerable<CadenaDeValor>> GetByObjetivoIdAsync(int objetivoId)
    {
        return await _context.CadenasDeValor
            .Include(c => c.Objetivo)
            .Include(c => c.Actividades)
            .Where(c => c.ObjetivoId == objetivoId)
            .ToListAsync();
    }

    public async Task<CadenaDeValor> CreateAsync(CadenaDeValor cadenaDeValor)
    {
        await _context.CadenasDeValor.AddAsync(cadenaDeValor);
        await _context.SaveChangesAsync();
        return cadenaDeValor;
    }

    public async Task<CadenaDeValor> UpdateAsync(CadenaDeValor cadenaDeValor)
    {
        _context.CadenasDeValor.Update(cadenaDeValor);
        await _context.SaveChangesAsync();
        return cadenaDeValor;
    }

    public async Task DeleteAsync(int id)
    {
        var cadenaDeValor = await _context.CadenasDeValor.FindAsync(id);
        if (cadenaDeValor != null)
        {
            _context.CadenasDeValor.Remove(cadenaDeValor);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.CadenasDeValor.AnyAsync(c => c.CadenaDeValorId == id);
    }
}
