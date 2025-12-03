using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class ServiciosTecnologicosRepository : IServiciosTecnologicosRepository
{
    private readonly ApplicationDbContext _context;
    public ServiciosTecnologicosRepository(ApplicationDbContext context) => _context = context;

    public async Task<List<ServiciosTecnologicos>> GetAllAsync() => await _context.Set<ServiciosTecnologicos>().AsNoTracking().ToListAsync();

    public async Task<ServiciosTecnologicos?> GetByIdAsync(int id) => await _context.Set<ServiciosTecnologicos>().FirstOrDefaultAsync(s => s.ServiciosTecnologicosId == id);

    public async Task<List<ServiciosTecnologicos>> GetByRubroIdAsync(int rubroId) => await _context.Set<ServiciosTecnologicos>().Where(s => s.RubroId == rubroId).AsNoTracking().ToListAsync();

    public async Task<ServiciosTecnologicos> CreateAsync(ServiciosTecnologicos entity)
    {
        _context.Set<ServiciosTecnologicos>().Add(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<ServiciosTecnologicos?> UpdateAsync(ServiciosTecnologicos entity)
    {
        var existing = await _context.Set<ServiciosTecnologicos>().FirstOrDefaultAsync(s => s.ServiciosTecnologicosId == entity.ServiciosTecnologicosId);
        if (existing == null) return null;
        existing.RubroId = entity.RubroId;
        existing.Descripcion = entity.Descripcion;
        existing.Total = entity.Total;
        existing.RagEstado = entity.RagEstado;
        existing.PeriodoNum = entity.PeriodoNum;
        existing.PeriodoTipo = entity.PeriodoTipo;
        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Set<ServiciosTecnologicos>().FirstOrDefaultAsync(s => s.ServiciosTecnologicosId == id);
        if (existing == null) return false;
        _context.Set<ServiciosTecnologicos>().Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
