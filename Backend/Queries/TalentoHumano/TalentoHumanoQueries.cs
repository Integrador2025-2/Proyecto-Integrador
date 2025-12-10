using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.TalentoHumano;

public record GetTalentoHumanoByIdQuery(int TalentoHumanoId) : IRequest<TalentoHumanoDto>;

public record GetAllTalentoHumanoQuery : IRequest<IEnumerable<TalentoHumanoDto>>;

public record GetTalentoHumanoByRecursoEspecificoIdQuery(int RecursoEspecificoId) : IRequest<IEnumerable<TalentoHumanoDto>>;
