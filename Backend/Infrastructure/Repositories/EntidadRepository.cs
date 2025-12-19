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
        return await _context.Entidades
            .Include(e => e.ActXEntidades)
            .FirstOrDefaultAsync(e => e.EntidadId == id);
    }

    public async Task<IEnumerable<Entidad>> GetAllAsync()
    {
        return await _context.Entidades
            .Include(e => e.ActXEntidades)
            .ToListAsync();
    }

    public async Task<Entidad> CreateAsync(Entidad entidad)
    {
        await _context.Entidades.AddAsync(entidad);
        await _context.SaveChangesAsync();
        return entidad;
    }

    public async Task<Entidad> UpdateAsync(Entidad entidad)
    {
        _context.Entidades.Update(entidad);
        await _context.SaveChangesAsync();
        return entidad;
    }

    public async Task DeleteAsync(int id)
    {
        var entidad = await _context.Entidades.FindAsync(id);
        if (entidad != null)
        {
            _context.Entidades.Remove(entidad);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Entidades.AnyAsync(e => e.EntidadId == id);
    }
}
