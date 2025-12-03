using MediatR;
using Backend.Models.DTOs;

namespace Backend.Queries.MaterialesInsumos;

public class GetAllMaterialesInsumosQuery : IRequest<List<MaterialesInsumosDto>> { }
