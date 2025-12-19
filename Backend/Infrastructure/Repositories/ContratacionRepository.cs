using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class ContratacionRepository : IContratacionRepository
{
    private readonly ApplicationDbContext _context;

    public ContratacionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Contratacion?> GetByIdAsync(int contratacionId)
    {
        return await _context.Contrataciones
            .Include(c => c.TalentosHumanos)
            .FirstOrDefaultAsync(c => c.ContratacionId == contratacionId);
    }

    public async Task<IEnumerable<Contratacion>> GetAllAsync()
    {
        return await _context.Contrataciones
            .Include(c => c.TalentosHumanos)
            .ToListAsync();
    }

    public async Task<IEnumerable<Contratacion>> GetByCategoriaAsync(string categoria)
    {
        return await _context.Contrataciones
            .Include(c => c.TalentosHumanos)
            .Where(c => c.Categoria == categoria)
            .ToListAsync();
    }

    public async Task<Contratacion> CreateAsync(Contratacion contratacion)
    {
        await _context.Contrataciones.AddAsync(contratacion);
        await _context.SaveChangesAsync();
        return contratacion;
    }

    public async Task<Contratacion> UpdateAsync(Contratacion contratacion)
    {
        _context.Contrataciones.Update(contratacion);
        await _context.SaveChangesAsync();
        return contratacion;
    }

    public async Task DeleteAsync(int contratacionId)
    {
        var contratacion = await _context.Contrataciones.FindAsync(contratacionId);
        if (contratacion != null)
        {
            _context.Contrataciones.Remove(contratacion);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int contratacionId)
    {
        return await _context.Contrataciones.AnyAsync(c => c.ContratacionId == contratacionId);
    }
}
