using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.EquiposSoftware;

public class GetAllEquiposSoftwareQuery : IRequest<List<EquiposSoftwareDto>> { }
