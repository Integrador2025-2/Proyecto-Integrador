namespace Backend.Models.DTOs;

public class RecursoEspecificoDto
{
    public int RecursoEspecificoId { get; set; }
    public int RecursoId { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Detalle { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Total { get; set; }
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}

public class CreateRecursoEspecificoDto
{
    public int RecursoId { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Detalle { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Total { get; set; }
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}

public class UpdateRecursoEspecificoDto
{
    public int RecursoEspecificoId { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Detalle { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Total { get; set; }
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}
