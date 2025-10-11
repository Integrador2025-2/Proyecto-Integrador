using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class MaterialesInsumosRepository : IMaterialesInsumosRepository
{
    private readonly ApplicationDbContext _context;
    public MaterialesInsumosRepository(ApplicationDbContext context) => _context = context;

    public async Task<List<MaterialesInsumos>> GetAllAsync() => await _context.Set<MaterialesInsumos>().AsNoTracking().ToListAsync();
    public async Task<MaterialesInsumos?> GetByIdAsync(int id) => await _context.Set<MaterialesInsumos>().FirstOrDefaultAsync(m => m.MaterialesInsumosId == id);
    public async Task<List<MaterialesInsumos>> GetByRubroIdAsync(int rubroId) => await _context.Set<MaterialesInsumos>().Where(m => m.RubroId == rubroId).AsNoTracking().ToListAsync();
    public async Task<MaterialesInsumos> CreateAsync(MaterialesInsumos entity)
    {
        _context.Set<MaterialesInsumos>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<MaterialesInsumos?> UpdateAsync(MaterialesInsumos entity)
    {
        var existing = await _context.Set<MaterialesInsumos>().FirstOrDefaultAsync(m => m.MaterialesInsumosId == entity.MaterialesInsumosId);
        if (existing == null) return null;

        existing.Materiales = entity.Materiales;
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
        var existing = await _context.Set<MaterialesInsumos>().FirstOrDefaultAsync(m => m.MaterialesInsumosId == id);
        if (existing == null) return false;
        _context.Set<MaterialesInsumos>().Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
