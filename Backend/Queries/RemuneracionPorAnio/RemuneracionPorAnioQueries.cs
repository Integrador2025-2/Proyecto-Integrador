using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.RemuneracionPorAnio;

public record GetRemuneracionPorAnioByIdQuery(int RemuneracionPorAnioId) : IRequest<RemuneracionPorAnioDto>;

public record GetAllRemuneracionPorAnioQuery : IRequest<IEnumerable<RemuneracionPorAnioDto>>;

public record GetRemuneracionPorAnioByTalentoHumanoIdQuery(int TalentoHumanoId) : IRequest<IEnumerable<RemuneracionPorAnioDto>>;

public record GetRemuneracionPorAnioByAnioQuery(int Anio) : IRequest<IEnumerable<RemuneracionPorAnioDto>>;
