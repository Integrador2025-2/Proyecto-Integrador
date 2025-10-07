using MediatR;

namespace Backend.Commands.Roles;

public class DeleteRoleCommand : IRequest<bool>
{
    public int Id { get; set; }
}
