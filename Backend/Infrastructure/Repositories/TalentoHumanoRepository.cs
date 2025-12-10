using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class TalentoHumanoRepository : ITalentoHumanoRepository
{
    private readonly ApplicationDbContext _context;

    public TalentoHumanoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<TalentoHumano?> GetByIdAsync(int talentoHumanoId)
    {
        return await _context.TalentoHumano
            .Include(th => th.RecursoEspecifico)
            .Include(th => th.Contratacion)
            .FirstOrDefaultAsync(th => th.TalentoHumanoId == talentoHumanoId);
    }

    public async Task<IEnumerable<TalentoHumano>> GetAllAsync()
    {
        return await _context.TalentoHumano
            .Include(th => th.RecursoEspecifico)
            .Include(th => th.Contratacion)
            .ToListAsync();
    }

    public async Task<IEnumerable<TalentoHumano>> GetByRecursoEspecificoIdAsync(int recursoEspecificoId)
    {
        return await _context.TalentoHumano
            .Include(th => th.RecursoEspecifico)
            .Include(th => th.Contratacion)
            .Where(th => th.RecursoEspecificoId == recursoEspecificoId)
            .ToListAsync();
    }

    public async Task<TalentoHumano> CreateAsync(TalentoHumano talentoHumano)
    {
        await _context.TalentoHumano.AddAsync(talentoHumano);
        await _context.SaveChangesAsync();
        return talentoHumano;
    }

    public async Task<TalentoHumano> UpdateAsync(TalentoHumano talentoHumano)
    {
        _context.TalentoHumano.Update(talentoHumano);
        await _context.SaveChangesAsync();
        return talentoHumano;
    }

    public async Task DeleteAsync(int talentoHumanoId)
    {
        var talentoHumano = await _context.TalentoHumano.FindAsync(talentoHumanoId);
        if (talentoHumano != null)
        {
            _context.TalentoHumano.Remove(talentoHumano);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int talentoHumanoId)
    {
        return await _context.TalentoHumano.AnyAsync(th => th.TalentoHumanoId == talentoHumanoId);
    }
}
