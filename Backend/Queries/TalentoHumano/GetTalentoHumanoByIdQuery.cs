using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.TalentoHumano;

public class GetTalentoHumanoByIdQuery : IRequest<TalentoHumanoDto?>
{
    public int TalentoHumanoId { get; }
    public GetTalentoHumanoByIdQuery(int id) => TalentoHumanoId = id;
}
