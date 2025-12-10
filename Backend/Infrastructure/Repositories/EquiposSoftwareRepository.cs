using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class EquiposSoftwareRepository : IEquiposSoftwareRepository
{
    private readonly ApplicationDbContext _context;
    public EquiposSoftwareRepository(ApplicationDbContext context) => _context = context;

    public async Task<List<EquiposSoftware>> GetAllAsync() => await _context.Set<EquiposSoftware>().AsNoTracking().ToListAsync();
    public async Task<EquiposSoftware?> GetByIdAsync(int id) => await _context.Set<EquiposSoftware>().FirstOrDefaultAsync(e => e.EquiposSoftwareId == id);
    public async Task<List<EquiposSoftware>> GetByRubroIdAsync(int rubroId) => await _context.Set<EquiposSoftware>().Where(e => e.RubroId == rubroId).AsNoTracking().ToListAsync();
    public async Task<EquiposSoftware> CreateAsync(EquiposSoftware entity)
    {
        _context.Set<EquiposSoftware>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<EquiposSoftware?> UpdateAsync(EquiposSoftware entity)
    {
        var existing = await _context.Set<EquiposSoftware>().FirstOrDefaultAsync(e => e.EquiposSoftwareId == entity.EquiposSoftwareId);
        if (existing == null) return null;

        existing.EspecificacionesTecnicas = entity.EspecificacionesTecnicas;
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
        var existing = await _context.Set<EquiposSoftware>().FirstOrDefaultAsync(e => e.EquiposSoftwareId == id);
        if (existing == null) return false;
        _context.Set<EquiposSoftware>().Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
