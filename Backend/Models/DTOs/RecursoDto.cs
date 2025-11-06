namespace Backend.Models.DTOs;

public class RecursoDto
{
    public int RecursoId { get; set; }
    public int ActividadId { get; set; }
    public int EntidadId { get; set; }
    public int RubroId { get; set; }
    public string TipoRecurso { get; set; } = string.Empty;
    public decimal MontoEfectivo { get; set; }
    public decimal MontoEspecie { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public string? ActividadNombre { get; set; }
    public string? EntidadNombre { get; set; }
    public string? RubroDescripcion { get; set; }
}

public class CreateRecursoDto
{
    public int ActividadId { get; set; }
    public int EntidadId { get; set; }
    public int RubroId { get; set; }
    public string TipoRecurso { get; set; } = string.Empty;
    public decimal MontoEfectivo { get; set; }
    public decimal MontoEspecie { get; set; }
    public string Descripcion { get; set; } = string.Empty;
}

public class UpdateRecursoDto
{
    public int RecursoId { get; set; }
    public int EntidadId { get; set; }
    public int RubroId { get; set; }
    public string TipoRecurso { get; set; } = string.Empty;
    public decimal MontoEfectivo { get; set; }
    public decimal MontoEspecie { get; set; }
    public string Descripcion { get; set; } = string.Empty;
}
