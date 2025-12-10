using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class ProyectoRepository : IProyectoRepository
{
    private readonly ApplicationDbContext _context;

    public ProyectoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Proyecto?> GetByIdAsync(int id)
    {
        return await _context.Proyectos
            .Include(p => p.Usuario)
            .Include(p => p.Objetivos)
            .FirstOrDefaultAsync(p => p.ProyectoId == id);
    }

    public async Task<IEnumerable<Proyecto>> GetAllAsync()
    {
        return await _context.Proyectos
            .Include(p => p.Usuario)
            .Include(p => p.Objetivos)
            .ToListAsync();
    }

    public async Task<IEnumerable<Proyecto>> GetByUsuarioIdAsync(int usuarioId)
    {
        return await _context.Proyectos
            .Include(p => p.Usuario)
            .Include(p => p.Objetivos)
            .Where(p => p.UsuarioId == usuarioId)
            .ToListAsync();
    }

    public async Task<Proyecto> CreateAsync(Proyecto proyecto)
    {
        await _context.Proyectos.AddAsync(proyecto);
        await _context.SaveChangesAsync();
        return proyecto;
    }

    public async Task<Proyecto> UpdateAsync(Proyecto proyecto)
    {
        _context.Proyectos.Update(proyecto);
        await _context.SaveChangesAsync();
        return proyecto;
    }

    public async Task DeleteAsync(int id)
    {
        var proyecto = await _context.Proyectos.FindAsync(id);
        if (proyecto != null)
        {
            _context.Proyectos.Remove(proyecto);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Proyectos.AnyAsync(p => p.ProyectoId == id);
    }
}
