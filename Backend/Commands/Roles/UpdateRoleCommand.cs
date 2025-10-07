using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.Roles;

public class UpdateRoleCommand : IRequest<RoleDto>
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Permissions { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}
