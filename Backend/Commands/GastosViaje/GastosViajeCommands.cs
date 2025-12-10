using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.GastosViaje;

public class CreateGastosViajeCommand : IRequest<GastosViajeDto>
{
    public int RecursoEspecificoId { get; set; }
    public decimal Costo { get; set; }
}

public class UpdateGastosViajeCommand : IRequest<GastosViajeDto>
{
    public int GastosViajeId { get; set; }
    public decimal Costo { get; set; }
}

public class DeleteGastosViajeCommand : IRequest<bool>
{
    public int GastosViajeId { get; set; }
}
