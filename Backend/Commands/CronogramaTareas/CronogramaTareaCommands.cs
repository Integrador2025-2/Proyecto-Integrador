using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.CronogramaTareas;

public class CreateCronogramaTareaCommand : IRequest<CronogramaTareaDto>
{
    public int TareaId { get; set; }
    public int DuracionMeses { get; set; }
    public int DuracionDias { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}

public class UpdateCronogramaTareaCommand : IRequest<CronogramaTareaDto>
{
    public int CronogramaId { get; set; }
    public int DuracionMeses { get; set; }
    public int DuracionDias { get; set; }
    public DateTime FechaInicio { get; set; }
    public DateTime FechaFin { get; set; }
}

public class DeleteCronogramaTareaCommand : IRequest<bool>
{
    public int CronogramaId { get; set; }
}
