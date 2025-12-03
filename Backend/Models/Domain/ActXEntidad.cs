namespace Backend.Models.Domain;

public class ActXEntidad
{
    public int ActXEntidadId { get; set; }

    // Foreign keys
    public int ActividadId { get; set; }
    public Actividad? Actividad { get; set; }

    public int EntidadId { get; set; }
    public Entidad? Entidad { get; set; }

    // Valores aportados por la entidad en la actividad
    public decimal Efectivo { get; set; }
    public decimal Especie { get; set; }
    
    // La columna rubro_id se mantiene en BD pero no será modelada en el domain si no la necesitas aquí
}