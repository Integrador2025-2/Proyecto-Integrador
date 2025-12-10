using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.CadenasDeValor;

public class CreateCadenaDeValorCommand : IRequest<CadenaDeValorDto>
{
    public int ObjetivoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string ObjetivoEspecifico { get; set; } = string.Empty;
}

public class UpdateCadenaDeValorCommand : IRequest<CadenaDeValorDto>
{
    public int CadenaDeValorId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string ObjetivoEspecifico { get; set; } = string.Empty;
}

public class DeleteCadenaDeValorCommand : IRequest<bool>
{
    public int CadenaDeValorId { get; set; }
}
