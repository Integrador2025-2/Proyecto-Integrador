using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class EquiposSoftwareRepository : IEquiposSoftwareRepository
{
    private readonly ApplicationDbContext _context;

    public EquiposSoftwareRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<EquiposSoftware?> GetByIdAsync(int equiposSoftwareId)
    {
        return await _context.EquiposSoftware
            .Include(es => es.RecursoEspecifico)
            .FirstOrDefaultAsync(es => es.EquiposSoftwareId == equiposSoftwareId);
    }

    public async Task<IEnumerable<EquiposSoftware>> GetAllAsync()
    {
        return await _context.EquiposSoftware
            .Include(es => es.RecursoEspecifico)
            .ToListAsync();
    }

    public async Task<EquiposSoftware?> GetByRecursoEspecificoIdAsync(int recursoEspecificoId)
    {
        return await _context.EquiposSoftware
            .Include(es => es.RecursoEspecifico)
            .FirstOrDefaultAsync(es => es.RecursoEspecificoId == recursoEspecificoId);
    }

    public async Task<EquiposSoftware> CreateAsync(EquiposSoftware equiposSoftware)
    {
        await _context.EquiposSoftware.AddAsync(equiposSoftware);
        await _context.SaveChangesAsync();
        return equiposSoftware;
    }

    public async Task<EquiposSoftware> UpdateAsync(EquiposSoftware equiposSoftware)
    {
        _context.EquiposSoftware.Update(equiposSoftware);
        await _context.SaveChangesAsync();
        return equiposSoftware;
    }

    public async Task<bool> DeleteAsync(int equiposSoftwareId)
    {
        var equiposSoftware = await _context.EquiposSoftware
            .FirstOrDefaultAsync(es => es.EquiposSoftwareId == equiposSoftwareId);
        
        if (equiposSoftware == null)
        {
            return false;
        }

        _context.EquiposSoftware.Remove(equiposSoftware);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int equiposSoftwareId)
    {
        return await _context.EquiposSoftware
            .AnyAsync(es => es.EquiposSoftwareId == equiposSoftwareId);
    }
}
