using MediatR;
using Backend.Models.DTOs;

namespace Backend.Commands.Rubros;

public class UpdateRubroCommand : IRequest<RubroDto?>
{
    public int RubroId { get; set; }
    public string Descripcion { get; set; } = string.Empty;
}
