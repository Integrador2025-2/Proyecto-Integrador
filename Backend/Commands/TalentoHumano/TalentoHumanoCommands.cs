using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.TalentoHumano;

public record CreateTalentoHumanoCommand(
    int RecursoEspecificoId,
    int ContratacionId,
    string CargoEspecifico,
    int Semanas,
    decimal Total
) : IRequest<TalentoHumanoDto>;

public record UpdateTalentoHumanoCommand(
    int TalentoHumanoId,
    int ContratacionId,
    string CargoEspecifico,
    int Semanas,
    decimal Total
) : IRequest<TalentoHumanoDto>;

public record DeleteTalentoHumanoCommand(int TalentoHumanoId) : IRequest;
