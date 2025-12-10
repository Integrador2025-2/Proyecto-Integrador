namespace Backend.Models.DTOs;

public class OtrosDto
{
    public int OtrosId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public string Justificacion { get; set; } = string.Empty;
}

public class CreateOtrosDto
{
    public int RecursoEspecificoId { get; set; }
    public string Justificacion { get; set; } = string.Empty;
}

public class UpdateOtrosDto
{
    public int OtrosId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public string Justificacion { get; set; } = string.Empty;
}
