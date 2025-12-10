using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class AdministrativosRepository : IAdministrativosRepository
{
    private readonly ApplicationDbContext _context;

    public AdministrativosRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Administrativos?> GetByIdAsync(int administrativoId)
    {
        return await _context.Administrativos
            .Include(a => a.RecursoEspecifico)
            .FirstOrDefaultAsync(a => a.AdministrativoId == administrativoId);
    }

    public async Task<IEnumerable<Administrativos>> GetAllAsync()
    {
        return await _context.Administrativos
            .Include(a => a.RecursoEspecifico)
            .ToListAsync();
    }

    public async Task<Administrativos?> GetByRecursoEspecificoIdAsync(int recursoEspecificoId)
    {
        return await _context.Administrativos
            .Include(a => a.RecursoEspecifico)
            .FirstOrDefaultAsync(a => a.RecursoEspecificoId == recursoEspecificoId);
    }

    public async Task<Administrativos> CreateAsync(Administrativos administrativos)
    {
        await _context.Administrativos.AddAsync(administrativos);
        await _context.SaveChangesAsync();
        return administrativos;
    }

    public async Task<Administrativos> UpdateAsync(Administrativos administrativos)
    {
        _context.Administrativos.Update(administrativos);
        await _context.SaveChangesAsync();
        return administrativos;
    }

    public async Task<bool> DeleteAsync(int administrativoId)
    {
        var administrativos = await _context.Administrativos
            .FirstOrDefaultAsync(a => a.AdministrativoId == administrativoId);
        
        if (administrativos == null)
        {
            return false;
        }

        _context.Administrativos.Remove(administrativos);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int administrativoId)
    {
        return await _context.Administrativos
            .AnyAsync(a => a.AdministrativoId == administrativoId);
    }
}
