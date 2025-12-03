using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class EntidadRepository : IEntidadRepository
{
    private readonly ApplicationDbContext _context;

    public EntidadRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Entidad?> GetByIdAsync(int id)
    {
        return await _context.Set<Entidad>().FindAsync(id);
    }

    public async Task<List<Entidad>> GetAllAsync()
    {
        return await _context.Set<Entidad>().OrderBy(e => e.Nombre).ToListAsync();
    }

    public async Task<List<Entidad>> GetByFilterAsync(string? searchTerm = null)
    {
        var q = _context.Set<Entidad>().AsQueryable();
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            q = q.Where(e => e.Nombre.Contains(searchTerm));
        }
        return await q.OrderBy(e => e.Nombre).ToListAsync();
    }

    public async Task<Entidad> CreateAsync(Entidad entidad)
    {
        _context.Set<Entidad>().Add(entidad);
        await _context.SaveChangesAsync();
        return entidad;
    }

    public async Task<Entidad> UpdateAsync(Entidad entidad)
    {
        _context.Set<Entidad>().Update(entidad);
        await _context.SaveChangesAsync();
        return entidad;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var e = await _context.Set<Entidad>().FindAsync(id);
        if (e == null) return false;
        _context.Set<Entidad>().Remove(e);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Set<Entidad>().AnyAsync(e => e.EntidadId == id);
    }
}
