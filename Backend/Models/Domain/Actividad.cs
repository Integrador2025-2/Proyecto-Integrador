using System.ComponentModel.DataAnnotations.Schema;
namespace Backend.Models.Domain;

public class Actividad
{
    public int ActividadId { get; set; }
    public int ProyectoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;

    // Totales por año (no mapeado por EF por simplicidad; considerar entidad propia si se requiere persistencia)
    [NotMapped]
    public List<decimal> TotalxAnios { get; set; } = new List<decimal>();
    public int CantidadAnios { get; set; }

    public string EspecificacionesTecnicas { get; set; } = string.Empty;
    public decimal ValorUnitario { get; set; }

    // ValorTotal se calcula a partir de los totales por año cuando están presentes
    public decimal ValorTotal
    {
        get
        {
            return TotalxAnios != null && TotalxAnios.Count > 0 ? TotalxAnios.Sum() : 0m;
        }
    }

    // Navigation collections: una actividad puede tener varios rubros asociados
    public List<Backend.Models.Domain.TalentoHumano>? TalentoHumano { get; set; }
    public List<Backend.Models.Domain.EquiposSoftware>? EquiposSoftware { get; set; }
    public List<Backend.Models.Domain.ServiciosTecnologicos>? ServiciosTecnologicos { get; set; }
    public List<Backend.Models.Domain.MaterialesInsumos>? MaterialesInsumos { get; set; }
    public List<Backend.Models.Domain.CapacitacionEventos>? CapacitacionEventos { get; set; }
    public List<Backend.Models.Domain.GastosViaje>? GastosViaje { get; set; }
    
    // Entidades participantes en la actividad (tabla intermedia)
    public List<Backend.Models.Domain.ActXEntidad>? ActXEntidades { get; set; }
}
