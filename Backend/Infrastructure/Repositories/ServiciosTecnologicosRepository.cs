using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class ServiciosTecnologicosRepository : IServiciosTecnologicosRepository
{
    private readonly ApplicationDbContext _context;

    public ServiciosTecnologicosRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<ServiciosTecnologicos?> GetByIdAsync(int serviciosTecnologicosId)
    {
        return await _context.ServiciosTecnologicos
            .Include(st => st.RecursoEspecifico)
            .FirstOrDefaultAsync(st => st.ServiciosTecnologicosId == serviciosTecnologicosId);
    }

    public async Task<IEnumerable<ServiciosTecnologicos>> GetAllAsync()
    {
        return await _context.ServiciosTecnologicos
            .Include(st => st.RecursoEspecifico)
            .ToListAsync();
    }

    public async Task<ServiciosTecnologicos?> GetByRecursoEspecificoIdAsync(int recursoEspecificoId)
    {
        return await _context.ServiciosTecnologicos
            .Include(st => st.RecursoEspecifico)
            .FirstOrDefaultAsync(st => st.RecursoEspecificoId == recursoEspecificoId);
    }

    public async Task<ServiciosTecnologicos> CreateAsync(ServiciosTecnologicos serviciosTecnologicos)
    {
        await _context.ServiciosTecnologicos.AddAsync(serviciosTecnologicos);
        await _context.SaveChangesAsync();
        return serviciosTecnologicos;
    }

    public async Task<ServiciosTecnologicos> UpdateAsync(ServiciosTecnologicos serviciosTecnologicos)
    {
        _context.ServiciosTecnologicos.Update(serviciosTecnologicos);
        await _context.SaveChangesAsync();
        return serviciosTecnologicos;
    }

    public async Task<bool> DeleteAsync(int serviciosTecnologicosId)
    {
        var serviciosTecnologicos = await _context.ServiciosTecnologicos
            .FirstOrDefaultAsync(st => st.ServiciosTecnologicosId == serviciosTecnologicosId);
        
        if (serviciosTecnologicos == null)
        {
            return false;
        }

        _context.ServiciosTecnologicos.Remove(serviciosTecnologicos);
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int serviciosTecnologicosId)
    {
        return await _context.ServiciosTecnologicos
            .AnyAsync(st => st.ServiciosTecnologicosId == serviciosTecnologicosId);
    }
}
