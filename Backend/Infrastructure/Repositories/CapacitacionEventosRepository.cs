using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class CapacitacionEventosRepository : ICapacitacionEventosRepository
{
    private readonly ApplicationDbContext _context;
    public CapacitacionEventosRepository(ApplicationDbContext context) => _context = context;

    public async Task<List<CapacitacionEventos>> GetAllAsync() => await _context.Set<CapacitacionEventos>().AsNoTracking().ToListAsync();

    public async Task<CapacitacionEventos?> GetByIdAsync(int id) => await _context.Set<CapacitacionEventos>().FirstOrDefaultAsync(c => c.CapacitacionEventosId == id);

    public async Task<List<CapacitacionEventos>> GetByRubroIdAsync(int rubroId) => await _context.Set<CapacitacionEventos>().Where(c => c.RubroId == rubroId).AsNoTracking().ToListAsync();

    public async Task<CapacitacionEventos> CreateAsync(CapacitacionEventos entity)
    {
        _context.Set<CapacitacionEventos>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<CapacitacionEventos?> UpdateAsync(CapacitacionEventos entity)
    {
        var existing = await _context.Set<CapacitacionEventos>().FirstOrDefaultAsync(c => c.CapacitacionEventosId == entity.CapacitacionEventosId);
        if (existing == null) return null;

        existing.Tema = entity.Tema;
        existing.Cantidad = entity.Cantidad;
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
        var existing = await _context.Set<CapacitacionEventos>().FirstOrDefaultAsync(c => c.CapacitacionEventosId == id);
        if (existing == null) return false;
        _context.Set<CapacitacionEventos>().Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
