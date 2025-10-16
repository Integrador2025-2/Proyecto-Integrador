using MediatR;
using Backend.Models.DTOs;

namespace Backend.Commands.CadenasDeValor;

public class CreateCadenaDeValorCommand : IRequest<CadenaDeValorDto>
{
    public string Nombre { get; set; } = string.Empty;
    public string ObjetivoEspecifico { get; set; } = string.Empty;
    public string Producto { get; set; } = string.Empty;
}
