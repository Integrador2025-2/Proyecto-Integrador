using Backend.Infrastructure.Context;
using Backend.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Backend.Infrastructure.Repositories;

public class ActividadRepository : IActividadRepository
{
    private readonly ApplicationDbContext _context;
    public ActividadRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Actividad>> GetAllAsync()
    {
        return await _context.Set<Actividad>()
            .Include(a => a.TalentoHumano)
            .Include(a => a.EquiposSoftware)
            .Include(a => a.ServiciosTecnologicos)
            .Include(a => a.MaterialesInsumos)
            .Include(a => a.CapacitacionEventos)
            .Include(a => a.GastosViaje)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Actividad?> GetByIdAsync(int id)
    {
        return await _context.Set<Actividad>()
            .Include(a => a.TalentoHumano)
            .Include(a => a.EquiposSoftware)
            .Include(a => a.ServiciosTecnologicos)
            .Include(a => a.MaterialesInsumos)
            .Include(a => a.CapacitacionEventos)
            .Include(a => a.GastosViaje)
            .FirstOrDefaultAsync(a => a.ActividadId == id);
    }

    public async Task<List<Actividad>> GetByProyectoIdAsync(int proyectoId)
    {
        return await _context.Set<Actividad>()
            .Where(a => a.ProyectoId == proyectoId)
            .Include(a => a.TalentoHumano)
            .Include(a => a.EquiposSoftware)
            .Include(a => a.ServiciosTecnologicos)
            .Include(a => a.MaterialesInsumos)
            .Include(a => a.CapacitacionEventos)
            .Include(a => a.GastosViaje)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Actividad> CreateAsync(Actividad actividad)
    {
        // Ensure EF tracks scalar properties; TotalxAnios is NotMapped and won't be persisted
        _context.Set<Actividad>().Add(actividad);
        await _context.SaveChangesAsync();
        return actividad;
    }

    public async Task<Actividad?> UpdateAsync(Actividad actividad)
    {
        var existing = await _context.Set<Actividad>().FirstOrDefaultAsync(a => a.ActividadId == actividad.ActividadId);
        if (existing == null) return null;

        existing.Nombre = actividad.Nombre;
        existing.Descripcion = actividad.Descripcion;
        existing.Justificacion = actividad.Justificacion;
        existing.ValorUnitario = actividad.ValorUnitario;
        existing.CantidadAnios = actividad.CantidadAnios;
        existing.EspecificacionesTecnicas = actividad.EspecificacionesTecnicas;
        // Note: TotalxAnios is NotMapped and not persisted by EF; if persistence is needed convert to entity
        existing.ProyectoId = actividad.ProyectoId;

        await _context.SaveChangesAsync();
        return existing;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var existing = await _context.Set<Actividad>().FirstOrDefaultAsync(a => a.ActividadId == id);
        if (existing == null) return false;
        _context.Set<Actividad>().Remove(existing);
        await _context.SaveChangesAsync();
        return true;
    }
}
