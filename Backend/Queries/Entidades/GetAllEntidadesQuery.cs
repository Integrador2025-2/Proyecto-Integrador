using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.Entidades;

public class GetAllEntidadesQuery : IRequest<List<EntidadDto>>
{
    public string? SearchTerm { get; set; }
}
