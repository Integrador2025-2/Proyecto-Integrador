using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.GastosViaje;

public class GetAllGastosViajeQuery : IRequest<List<GastosViajeDto>> { }
