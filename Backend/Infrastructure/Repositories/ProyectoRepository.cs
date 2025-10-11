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

    public async Task<List<Proyecto>> GetAllAsync()
    {
        return await _context.Proyectos.AsNoTracking().ToListAsync();
    }

    public async Task<Proyecto?> GetByIdAsync(int id)
    {
        return await _context.Proyectos.FirstOrDefaultAsync(p => p.ProyectoId == id);
    }

    public async Task<Proyecto> CreateAsync(Proyecto proyecto)
    {
        _context.Proyectos.Add(proyecto);
        await _context.SaveChangesAsync();
        return proyecto;
    }
}
