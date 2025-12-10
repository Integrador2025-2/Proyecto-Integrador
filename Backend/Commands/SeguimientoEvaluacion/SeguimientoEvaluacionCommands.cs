using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.SeguimientoEvaluacion;

public class CreateSeguimientoEvaluacionCommand : IRequest<SeguimientoEvaluacionDto>
{
    public int RecursoEspecificoId { get; set; }
    public string CargoResponsable { get; set; } = string.Empty;
    public string MetodoEvaluacion { get; set; } = string.Empty;
    public string Frecuencia { get; set; } = string.Empty;
}

public class UpdateSeguimientoEvaluacionCommand : IRequest<SeguimientoEvaluacionDto>
{
    public int SeguimientoId { get; set; }
    public string CargoResponsable { get; set; } = string.Empty;
    public string MetodoEvaluacion { get; set; } = string.Empty;
    public string Frecuencia { get; set; } = string.Empty;
}

public class DeleteSeguimientoEvaluacionCommand : IRequest<bool>
{
    public int SeguimientoId { get; set; }
}
