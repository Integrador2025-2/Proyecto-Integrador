using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class RubroRepository : IRubroRepository
{
    private readonly ApplicationDbContext _context;

    public RubroRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Rubro?> GetByIdAsync(int id)
    {
        return await _context.Rubros.FindAsync(id);
    }

    public async Task<IEnumerable<Rubro>> GetAllAsync()
    {
        return await _context.Rubros.ToListAsync();
    }

    public async Task<Rubro> CreateAsync(Rubro rubro)
    {
        _context.Rubros.Add(rubro);
        await _context.SaveChangesAsync();
        return rubro;
    }

    public async Task<Rubro> UpdateAsync(Rubro rubro)
    {
        _context.Rubros.Update(rubro);
        await _context.SaveChangesAsync();
        return rubro;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var rubro = await _context.Rubros.FindAsync(id);
        if (rubro == null)
            return false;

        _context.Rubros.Remove(rubro);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Rubros.AnyAsync(r => r.RubroId == id);
    }
}
