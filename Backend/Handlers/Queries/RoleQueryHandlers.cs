using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Roles;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetRoleByIdQueryHandler : IRequestHandler<GetRoleByIdQuery, RoleDto?>
{
    private readonly IRoleRepository _roleRepository;

    public GetRoleByIdQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<RoleDto?> Handle(GetRoleByIdQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id);
        if (role == null) return null;

        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            Permissions = role.Permissions,
            IsActive = role.IsActive,
            CreatedAt = role.CreatedAt,
            UpdatedAt = role.UpdatedAt
        };
    }
}

public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, IEnumerable<RoleDto>>
{
    private readonly IRoleRepository _roleRepository;

    public GetAllRolesQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<IEnumerable<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetAllAsync();

        return roles.Select(role => new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            Permissions = role.Permissions,
            IsActive = role.IsActive,
            CreatedAt = role.CreatedAt,
            UpdatedAt = role.UpdatedAt
        });
    }
}

public class GetRoleByNameQueryHandler : IRequestHandler<GetRoleByNameQuery, RoleDto?>
{
    private readonly IRoleRepository _roleRepository;

    public GetRoleByNameQueryHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<RoleDto?> Handle(GetRoleByNameQuery request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByNameAsync(request.Name);
        if (role == null) return null;

        return new RoleDto
        {
            Id = role.Id,
            Name = role.Name,
            Description = role.Description,
            Permissions = role.Permissions,
            IsActive = role.IsActive,
            CreatedAt = role.CreatedAt,
            UpdatedAt = role.UpdatedAt
        };
    }
}

public class GetUsersByRoleIdQueryHandler : IRequestHandler<GetUsersByRoleIdQuery, IEnumerable<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersByRoleIdQueryHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetUsersByRoleIdQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetByRoleIdAsync(request.RoleId);

        return users.Select(user => new UserDto
        {
            Id = user.Id,
            FullName = user.FullName,
            Email = user.Email,
            IsActive = user.IsActive,
            RoleId = user.RoleId,
            RoleName = user.Role?.Name ?? "",
            Provider = user.Provider ?? "local"
        });
    }
}
