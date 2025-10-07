using MediatR;

namespace Backend.Commands.Users;

public class DeleteUserCommand : IRequest<bool>
{
    public int Id { get; set; }
}


