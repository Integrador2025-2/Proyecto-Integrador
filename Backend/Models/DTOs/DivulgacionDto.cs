namespace Backend.Models.DTOs;

public class DivulgacionDto
{
    public int DivulgacionId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public string MedioDivulgacion { get; set; } = string.Empty;
    public string Alcance { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
}

public class CreateDivulgacionDto
{
    public int RecursoEspecificoId { get; set; }
    public string MedioDivulgacion { get; set; } = string.Empty;
    public string Alcance { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
}

public class UpdateDivulgacionDto
{
    public int DivulgacionId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public string MedioDivulgacion { get; set; } = string.Empty;
    public string Alcance { get; set; } = string.Empty;
    public string Justificacion { get; set; } = string.Empty;
}
