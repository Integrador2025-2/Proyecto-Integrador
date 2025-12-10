using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class ObjetivoRepository : IObjetivoRepository
{
    private readonly ApplicationDbContext _context;

    public ObjetivoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Objetivo?> GetByIdAsync(int id)
    {
        return await _context.Objetivos
            .Include(o => o.Proyecto)
            .Include(o => o.CadenasDeValor)
            .FirstOrDefaultAsync(o => o.ObjetivoId == id);
    }

    public async Task<IEnumerable<Objetivo>> GetAllAsync()
    {
        return await _context.Objetivos
            .Include(o => o.Proyecto)
            .Include(o => o.CadenasDeValor)
            .ToListAsync();
    }

    public async Task<IEnumerable<Objetivo>> GetByProyectoIdAsync(int proyectoId)
    {
        return await _context.Objetivos
            .Include(o => o.Proyecto)
            .Include(o => o.CadenasDeValor)
            .Where(o => o.ProyectoId == proyectoId)
            .ToListAsync();
    }

    public async Task<Objetivo> CreateAsync(Objetivo objetivo)
    {
        await _context.Objetivos.AddAsync(objetivo);
        await _context.SaveChangesAsync();
        return objetivo;
    }

    public async Task<Objetivo> UpdateAsync(Objetivo objetivo)
    {
        _context.Objetivos.Update(objetivo);
        await _context.SaveChangesAsync();
        return objetivo;
    }

    public async Task DeleteAsync(int id)
    {
        var objetivo = await _context.Objetivos.FindAsync(id);
        if (objetivo != null)
        {
            _context.Objetivos.Remove(objetivo);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Objetivos.AnyAsync(o => o.ObjetivoId == id);
    }
}
