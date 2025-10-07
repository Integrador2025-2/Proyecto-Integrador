using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.Roles;

public class AssignRoleToUserCommand : IRequest<UserDto>
{
    public int UserId { get; set; }
    public int RoleId { get; set; }
}
