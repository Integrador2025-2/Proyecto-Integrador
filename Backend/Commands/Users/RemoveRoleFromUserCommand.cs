using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.Users;

public class RemoveRoleFromUserCommand : IRequest<UserDto>
{
    public int UserId { get; set; }
}
