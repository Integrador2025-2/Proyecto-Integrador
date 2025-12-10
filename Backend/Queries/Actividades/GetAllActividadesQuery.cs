using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.Actividades;

public class GetAllActividadesQuery : IRequest<List<ActividadDto>> { }
