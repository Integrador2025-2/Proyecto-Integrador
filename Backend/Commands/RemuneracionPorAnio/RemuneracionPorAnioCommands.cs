using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.RemuneracionPorAnio;

public record CreateRemuneracionPorAnioCommand(
    int TalentoHumanoId,
    int Anio,
    decimal Honorarios,
    decimal ValorHora,
    int SemanasAnio,
    decimal TotalAnio
) : IRequest<RemuneracionPorAnioDto>;

public record UpdateRemuneracionPorAnioCommand(
    int RemuneracionPorAnioId,
    int TalentoHumanoId,
    int Anio,
    decimal Honorarios,
    decimal ValorHora,
    int SemanasAnio,
    decimal TotalAnio
) : IRequest<RemuneracionPorAnioDto>;

public record DeleteRemuneracionPorAnioCommand(int RemuneracionPorAnioId) : IRequest;
