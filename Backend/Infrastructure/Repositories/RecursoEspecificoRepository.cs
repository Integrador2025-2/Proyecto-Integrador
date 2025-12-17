using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class RecursoEspecificoRepository : IRecursoEspecificoRepository
{
    private readonly ApplicationDbContext _context;

    public RecursoEspecificoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<RecursoEspecifico?> GetByIdAsync(int id)
    {
        return await _context.RecursosEspecificos
            .Include(re => re.Recurso)
            .FirstOrDefaultAsync(re => re.RecursoEspecificoId == id);
    }

    public async Task<IEnumerable<RecursoEspecifico>> GetAllAsync()
    {
        return await _context.RecursosEspecificos
            .Include(re => re.Recurso)
            .ToListAsync();
    }

    public async Task<IEnumerable<RecursoEspecifico>> GetByRecursoIdAsync(int recursoId)
    {
        return await _context.RecursosEspecificos
            .Include(re => re.Recurso)
            .Where(re => re.RecursoId == recursoId)
            .ToListAsync();
    }

    public async Task<IEnumerable<RecursoEspecifico>> GetByTipoAsync(string tipo)
    {
        return await _context.RecursosEspecificos
            .Include(re => re.Recurso)
            .Where(re => re.Tipo == tipo)
            .ToListAsync();
    }

    public async Task<RecursoEspecifico> CreateAsync(RecursoEspecifico recursoEspecifico)
    {
        _context.RecursosEspecificos.Add(recursoEspecifico);
        await _context.SaveChangesAsync();
        return recursoEspecifico;
    }

    public async Task<RecursoEspecifico> UpdateAsync(RecursoEspecifico recursoEspecifico)
    {
        _context.RecursosEspecificos.Update(recursoEspecifico);
        await _context.SaveChangesAsync();
        return recursoEspecifico;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var recursoEspecifico = await _context.RecursosEspecificos.FindAsync(id);
        if (recursoEspecifico == null)
            return false;

        _context.RecursosEspecificos.Remove(recursoEspecifico);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.RecursosEspecificos.AnyAsync(re => re.RecursoEspecificoId == id);
    }
}
