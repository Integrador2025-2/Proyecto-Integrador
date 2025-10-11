using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.GastosViaje;

public class GetGastosViajeByIdQuery : IRequest<GastosViajeDto?>
{
    public int Id { get; }
    public GetGastosViajeByIdQuery(int id) => Id = id;
}
