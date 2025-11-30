using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.Roles;

public record CreateRoleCommand(
    string Name,
    string Description,
    string Permissions
) : IRequest<RoleDto>;

public record UpdateRoleCommand(
    int Id,
    string Name,
    string Description,
    string Permissions,
    bool IsActive
) : IRequest<RoleDto>;

public record DeleteRoleCommand(int Id) : IRequest<bool>;

public record AssignRoleToUserCommand(
    int UserId,
    int RoleId
) : IRequest<bool>;
