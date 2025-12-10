using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.MaterialesInsumos;

public class CreateMaterialesInsumosCommand : IRequest<MaterialesInsumosDto>
{
    public int RecursoEspecificoId { get; set; }
    public string Materiales { get; set; } = string.Empty;
}

public class UpdateMaterialesInsumosCommand : IRequest<MaterialesInsumosDto>
{
    public int MaterialesInsumosId { get; set; }
    public string Materiales { get; set; } = string.Empty;
}

public class DeleteMaterialesInsumosCommand : IRequest<bool>
{
    public int MaterialesInsumosId { get; set; }
}
