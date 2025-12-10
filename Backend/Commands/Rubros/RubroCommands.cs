using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.Rubros;

public class CreateRubroCommand : IRequest<RubroDto>
{
    public string Descripcion { get; set; } = string.Empty;
}

public class UpdateRubroCommand : IRequest<RubroDto>
{
    public int RubroId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
}

public class DeleteRubroCommand : IRequest<bool>
{
    public int RubroId { get; set; }
}
