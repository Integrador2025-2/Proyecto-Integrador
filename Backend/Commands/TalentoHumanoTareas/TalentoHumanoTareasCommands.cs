using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.TalentoHumanoTareas;

public record CreateTalentoHumanoTareaCommand(
    int TalentoHumanoId,
    int Tarea,
    int HorasAsignadas,
    string RolenTarea,
    string Observaciones,
    DateTime FechaAsignacion
) : IRequest<TalentoHumanoTareaDto>;

public record UpdateTalentoHumanoTareaCommand(
    int TalentoHumanoTareasId,
    int TalentoHumanoId,
    int Tarea,
    int HorasAsignadas,
    string RolenTarea,
    string Observaciones,
    DateTime FechaAsignacion
) : IRequest<TalentoHumanoTareaDto>;

public record DeleteTalentoHumanoTareaCommand(int TalentoHumanoTareasId) : IRequest;
