using System.ComponentModel.DataAnnotations;
namespace Backend.Models.Domain;

public class RecursoEspecifico
{
    [Key]
    public int RecursoEspecificoId { get; set; }
    public int RecursoId { get; set; }
    public string Tipo { get; set; } = string.Empty; // Ej: Talento Humano, Software, Material, etc.
    public string Detalle { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Total { get; set; }
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;

    // Navigation properties
    public Recurso? Recurso { get; set; }
    
    // Relaciones específicas (solo una será usada según el Tipo)
    public TalentoHumano? TalentoHumano { get; set; }
    public EquiposSoftware? EquiposSoftware { get; set; }
    public ServiciosTecnologicos? ServiciosTecnologicos { get; set; }
    public MaterialesInsumos? MaterialesInsumos { get; set; }
    public CapacitacionEventos? CapacitacionEventos { get; set; }
    public GastosViaje? GastosViaje { get; set; }
    public Infraestructura? Infraestructura { get; set; }
    public Administrativos? Administrativos { get; set; }
    public ProteccionConocimientoDivulgacion? ProteccionConocimientoDivulgacion { get; set; }
    public SeguimientoEvaluacion? SeguimientoEvaluacion { get; set; }
    public Divulgacion? Divulgacion { get; set; }
    public Otros? Otros { get; set; }
}
