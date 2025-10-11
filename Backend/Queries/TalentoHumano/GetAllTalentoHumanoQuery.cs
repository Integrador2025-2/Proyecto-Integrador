using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.TalentoHumano;

public class GetAllTalentoHumanoQuery : IRequest<List<TalentoHumanoDto>> { }
