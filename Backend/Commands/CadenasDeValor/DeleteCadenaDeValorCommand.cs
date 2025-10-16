using MediatR;

namespace Backend.Commands.CadenasDeValor;

public class DeleteCadenaDeValorCommand : IRequest<bool>
{
    public int CadenaDeValorId { get; set; }
}
