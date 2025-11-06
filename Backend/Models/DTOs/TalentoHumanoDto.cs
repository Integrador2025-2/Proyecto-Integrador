namespace Backend.Models.DTOs;

public class TalentoHumanoDto
{
    public int TalentoHumanoId { get; set; }
    public int RecursoEspecificoId { get; set; }
    public int ContratacionId { get; set; }
    public string CargoEspecifico { get; set; } = string.Empty;
    public int Semanas { get; set; }
    public decimal Total { get; set; }
}

public class CreateTalentoHumanoDto
{
    public int RecursoEspecificoId { get; set; }
    public int ContratacionId { get; set; }
    public string CargoEspecifico { get; set; } = string.Empty;
    public int Semanas { get; set; }
    public decimal Total { get; set; }
}

public class UpdateTalentoHumanoDto
{
    public int TalentoHumanoId { get; set; }
    public int ContratacionId { get; set; }
    public string CargoEspecifico { get; set; } = string.Empty;
    public int Semanas { get; set; }
    public decimal Total { get; set; }
}
