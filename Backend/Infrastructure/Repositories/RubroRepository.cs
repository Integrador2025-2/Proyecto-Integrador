using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class RubroRepository : IRubroRepository
{
    private readonly ApplicationDbContext _context;
    public RubroRepository(ApplicationDbContext context) => _context = context;

    public async Task<List<Rubro>> GetAllAsync() => await _context.Set<Rubro>().AsNoTracking().ToListAsync();

    public async Task<Rubro?> GetByIdAsync(int id) => await _context.Set<Rubro>().FirstOrDefaultAsync(r => r.RubroId == id);

    public async Task<Rubro> CreateAsync(Rubro rubro)
    {
        _context.Set<Rubro>().Add(rubro);
        await _context.SaveChangesAsync();
        return rubro;
    }

    public async Task<Rubro?> UpdateAsync(Rubro rubro)
    {
        var existing = await _context.Set<Rubro>().FirstOrDefaultAsync(r => r.RubroId == rubro.RubroId);
        if (existing == null) return null;
        existing.Descripcion = rubro.Descripcion;
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Set<Rubro>().FirstOrDefaultAsync(r => r.RubroId == id);
        if (existing == null) return false;
        _context.Set<Rubro>().Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
