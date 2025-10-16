using MediatR;
using Backend.Models.DTOs;

namespace Backend.Commands.CadenasDeValor;

public class UpdateCadenaDeValorCommand : IRequest<CadenaDeValorDto>
{
    public int CadenaDeValorId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string ObjetivoEspecifico { get; set; } = string.Empty;
    public string Producto { get; set; } = string.Empty;
}
