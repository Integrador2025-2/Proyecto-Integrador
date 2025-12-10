namespace Backend.Models.DTOs;

public class CronogramaTareaDto
{
    public int CronogramaId { get; set; }
    public int TareaId { get; set; }
    public int DuracionMeses { get; set; }
    public int DuracionDias { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
    public string? TareaNombre { get; set; }
}

public class CreateCronogramaTareaDto
{
    public int TareaId { get; set; }
    public int DuracionMeses { get; set; }
    public int DuracionDias { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}

public class UpdateCronogramaTareaDto
{
    public int CronogramaId { get; set; }
    public int DuracionMeses { get; set; }
    public int DuracionDias { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}
