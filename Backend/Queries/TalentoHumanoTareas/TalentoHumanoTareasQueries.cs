using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.TalentoHumanoTareas;

public record GetTalentoHumanoTareaByIdQuery(int TalentoHumanoTareasId) : IRequest<TalentoHumanoTareaDto>;

public record GetAllTalentoHumanoTareasQuery : IRequest<IEnumerable<TalentoHumanoTareaDto>>;

public record GetTalentoHumanoTareasByTalentoHumanoIdQuery(int TalentoHumanoId) : IRequest<IEnumerable<TalentoHumanoTareaDto>>;

public record GetTalentoHumanoTareasByTareaIdQuery(int TareaId) : IRequest<IEnumerable<TalentoHumanoTareaDto>>;
