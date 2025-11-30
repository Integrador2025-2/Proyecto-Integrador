using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.Roles;

public record GetRoleByIdQuery(int Id) : IRequest<RoleDto?>;

public record GetAllRolesQuery : IRequest<IEnumerable<RoleDto>>;

public record GetRoleByNameQuery(string Name) : IRequest<RoleDto?>;

public record GetUsersByRoleIdQuery(int RoleId) : IRequest<IEnumerable<UserDto>>;
