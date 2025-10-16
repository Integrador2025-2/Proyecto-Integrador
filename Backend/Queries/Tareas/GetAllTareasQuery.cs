using MediatR;
using Backend.Models.DTOs;
using System.Collections.Generic;

namespace Backend.Queries.Tareas;

public class GetAllTareasQuery : IRequest<IEnumerable<TareaDto>>
{
}
