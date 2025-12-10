namespace Backend.Models.DTOs;

public class InfraestructuraDto
{
    public int InfraestructuraId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public string TipoInfraestructura { get; set; } = string.Empty;
    public string Enlace { get; set; } = string.Empty;
    public string CaracteristicasTecnicas { get; set; } = string.Empty;
}

public class CreateInfraestructuraDto
{
    public int RecursoEspecificoId { get; set; }
    public string TipoInfraestructura { get; set; } = string.Empty;
    public string Enlace { get; set; } = string.Empty;
    public string CaracteristicasTecnicas { get; set; } = string.Empty;
}

public class UpdateInfraestructuraDto
{
    public int InfraestructuraId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public string TipoInfraestructura { get; set; } = string.Empty;
    public string Enlace { get; set; } = string.Empty;
    public string CaracteristicasTecnicas { get; set; } = string.Empty;
}
