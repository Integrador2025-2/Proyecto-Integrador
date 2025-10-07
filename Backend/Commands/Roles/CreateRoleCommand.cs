using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.Roles;

public class CreateRoleCommand : IRequest<RoleDto>
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Permissions { get; set; } = string.Empty;
}
