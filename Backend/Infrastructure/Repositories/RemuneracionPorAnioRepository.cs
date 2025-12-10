using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class RemuneracionPorAnioRepository : IRemuneracionPorAnioRepository
{
    private readonly ApplicationDbContext _context;

    public RemuneracionPorAnioRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RemuneracionPorAnio?> GetByIdAsync(int remuneracionPorAnioId)
    {
        return await _context.RemuneracionesPorAnio
            .Include(r => r.TalentoHumano)
            .FirstOrDefaultAsync(r => r.RemuneracionPorAnioId == remuneracionPorAnioId);
    }

    public async Task<IEnumerable<RemuneracionPorAnio>> GetAllAsync()
    {
        return await _context.RemuneracionesPorAnio
            .Include(r => r.TalentoHumano)
            .ToListAsync();
    }

    public async Task<IEnumerable<RemuneracionPorAnio>> GetByTalentoHumanoIdAsync(int talentoHumanoId)
    {
        return await _context.RemuneracionesPorAnio
            .Include(r => r.TalentoHumano)
            .Where(r => r.TalentoHumanoId == talentoHumanoId)
            .ToListAsync();
    }

    public async Task<IEnumerable<RemuneracionPorAnio>> GetByAnioAsync(int anio)
    {
        return await _context.RemuneracionesPorAnio
            .Include(r => r.TalentoHumano)
            .Where(r => r.Anio == anio)
            .ToListAsync();
    }

    public async Task<RemuneracionPorAnio> CreateAsync(RemuneracionPorAnio remuneracionPorAnio)
    {
        await _context.RemuneracionesPorAnio.AddAsync(remuneracionPorAnio);
        await _context.SaveChangesAsync();
        return remuneracionPorAnio;
    }

    public async Task<RemuneracionPorAnio> UpdateAsync(RemuneracionPorAnio remuneracionPorAnio)
    {
        _context.RemuneracionesPorAnio.Update(remuneracionPorAnio);
        await _context.SaveChangesAsync();
        return remuneracionPorAnio;
    }

    public async Task DeleteAsync(int remuneracionPorAnioId)
    {
        var remuneracionPorAnio = await _context.RemuneracionesPorAnio.FindAsync(remuneracionPorAnioId);
        if (remuneracionPorAnio != null)
        {
            _context.RemuneracionesPorAnio.Remove(remuneracionPorAnio);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int remuneracionPorAnioId)
    {
        return await _context.RemuneracionesPorAnio.AnyAsync(r => r.RemuneracionPorAnioId == remuneracionPorAnioId);
    }
}
