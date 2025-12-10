using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class MaterialesInsumosRepository : IMaterialesInsumosRepository
{
    private readonly ApplicationDbContext _context;

    public MaterialesInsumosRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<MaterialesInsumos?> GetByIdAsync(int materialesInsumosId)
    {
        return await _context.MaterialesInsumos
            .Include(mi => mi.RecursoEspecifico)
            .FirstOrDefaultAsync(mi => mi.MaterialesInsumosId == materialesInsumosId);
    }

    public async Task<IEnumerable<MaterialesInsumos>> GetAllAsync()
    {
        return await _context.MaterialesInsumos
            .Include(mi => mi.RecursoEspecifico)
            .ToListAsync();
    }

    public async Task<MaterialesInsumos?> GetByRecursoEspecificoIdAsync(int recursoEspecificoId)
    {
        return await _context.MaterialesInsumos
            .Include(mi => mi.RecursoEspecifico)
            .FirstOrDefaultAsync(mi => mi.RecursoEspecificoId == recursoEspecificoId);
    }

    public async Task<MaterialesInsumos> CreateAsync(MaterialesInsumos materialesInsumos)
    {
        await _context.MaterialesInsumos.AddAsync(materialesInsumos);
        await _context.SaveChangesAsync();
        return materialesInsumos;
    }

    public async Task<MaterialesInsumos> UpdateAsync(MaterialesInsumos materialesInsumos)
    {
        _context.MaterialesInsumos.Update(materialesInsumos);
        await _context.SaveChangesAsync();
        return materialesInsumos;
    }

    public async Task<bool> DeleteAsync(int materialesInsumosId)
    {
        var materialesInsumos = await _context.MaterialesInsumos
            .FirstOrDefaultAsync(mi => mi.MaterialesInsumosId == materialesInsumosId);
        
        if (materialesInsumos == null)
        {
            return false;
        }

        _context.MaterialesInsumos.Remove(materialesInsumos);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int materialesInsumosId)
    {
        return await _context.MaterialesInsumos
            .AnyAsync(mi => mi.MaterialesInsumosId == materialesInsumosId);
    }
}
