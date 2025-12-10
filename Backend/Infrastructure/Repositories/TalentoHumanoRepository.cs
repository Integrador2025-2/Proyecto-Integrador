using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class TalentoHumanoRepository : ITalentoHumanoRepository
{
    private readonly ApplicationDbContext _context;
    public TalentoHumanoRepository(ApplicationDbContext context) => _context = context;

    public async Task<List<TalentoHumano>> GetAllAsync() => await _context.Set<TalentoHumano>().AsNoTracking().ToListAsync();
    public async Task<TalentoHumano?> GetByIdAsync(int id) => await _context.Set<TalentoHumano>().FirstOrDefaultAsync(t => t.TalentoHumanoId == id);
    public async Task<List<TalentoHumano>> GetByRubroIdAsync(int rubroId) => await _context.Set<TalentoHumano>().Where(t => t.RubroId == rubroId).AsNoTracking().ToListAsync();
    public async Task<TalentoHumano> CreateAsync(TalentoHumano entity) { _context.Set<TalentoHumano>().Add(entity); await _context.SaveChangesAsync(); return entity; }
    public async Task<TalentoHumano?> UpdateAsync(TalentoHumano entity)
    {
        var existing = await _context.Set<TalentoHumano>().FirstOrDefaultAsync(t => t.TalentoHumanoId == entity.TalentoHumanoId);
        if (existing == null) return null;

        existing.CargoEspecifico = entity.CargoEspecifico;
        existing.Semanas = entity.Semanas;
        existing.Total = entity.Total;
        existing.RagEstado = entity.RagEstado;
        existing.PeriodoNum = entity.PeriodoNum;
        existing.PeriodoTipo = entity.PeriodoTipo;
        existing.RubroId = entity.RubroId;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Set<TalentoHumano>().FirstOrDefaultAsync(t => t.TalentoHumanoId == id);
        if (existing == null) return false;
        _context.Set<TalentoHumano>().Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
