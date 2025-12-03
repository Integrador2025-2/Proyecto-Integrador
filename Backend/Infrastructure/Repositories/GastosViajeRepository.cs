using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class GastosViajeRepository : IGastosViajeRepository
{
    private readonly ApplicationDbContext _context;
    public GastosViajeRepository(ApplicationDbContext context) => _context = context;

    public async Task<List<GastosViaje>> GetAllAsync() => await _context.Set<GastosViaje>().AsNoTracking().ToListAsync();

    public async Task<GastosViaje?> GetByIdAsync(int id) => await _context.Set<GastosViaje>().FirstOrDefaultAsync(g => g.GastosViajeId == id);

    public async Task<List<GastosViaje>> GetByRubroIdAsync(int rubroId) => await _context.Set<GastosViaje>().Where(g => g.RubroId == rubroId).AsNoTracking().ToListAsync();

    public async Task<GastosViaje> CreateAsync(GastosViaje entity)
    {
        _context.Set<GastosViaje>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<GastosViaje?> UpdateAsync(GastosViaje entity)
    {
        var existing = await _context.Set<GastosViaje>().FirstOrDefaultAsync(g => g.GastosViajeId == entity.GastosViajeId);
        if (existing == null) return null;

        existing.Costo = entity.Costo;
        existing.RagEstado = entity.RagEstado;
        existing.PeriodoNum = entity.PeriodoNum;
        existing.PeriodoTipo = entity.PeriodoTipo;
        existing.RubroId = entity.RubroId;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Set<GastosViaje>().FirstOrDefaultAsync(g => g.GastosViajeId == id);
        if (existing == null) return false;
        _context.Set<GastosViaje>().Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
