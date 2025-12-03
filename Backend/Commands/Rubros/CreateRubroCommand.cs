using MediatR;
using Backend.Models.DTOs;

namespace Backend.Commands.Rubros;

public class CreateRubroCommand : IRequest<RubroDto>
{
    public string Descripcion { get; set; } = string.Empty;
}
