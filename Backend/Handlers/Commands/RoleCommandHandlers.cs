using Backend.Commands.Roles;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, RoleDto>
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<CreateRoleCommandHandler> _logger;

    public CreateRoleCommandHandler(
        IRoleRepository roleRepository,
        ILogger<CreateRoleCommandHandler> logger)
    {
        _roleRepository = roleRepository;
        _logger = logger;
    }

    public async Task<RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        var existingRole = await _roleRepository.GetByNameAsync(request.Name);
        if (existingRole != null)
        {
            throw new InvalidOperationException($"Ya existe un rol con el nombre '{request.Name}'");
        }

        var role = new Role
        {
            Name = request.Name,
            Description = request.Description,
            Permissions = request.Permissions,
            CreatedAt = DateTime.UtcNow,
            IsActive = true
        };

        var createdRole = await _roleRepository.CreateAsync(role);
        _logger.LogInformation("Rol creado: {RoleId} - {RoleName}", createdRole.Id, createdRole.Name);

        return new RoleDto
        {
            Id = createdRole.Id,
            Name = createdRole.Name,
            Description = createdRole.Description,
            Permissions = createdRole.Permissions,
            IsActive = createdRole.IsActive,
            CreatedAt = createdRole.CreatedAt,
            UpdatedAt = createdRole.UpdatedAt
        };
    }
}

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, RoleDto>
{
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<UpdateRoleCommandHandler> _logger;

    public UpdateRoleCommandHandler(
        IRoleRepository roleRepository,
        ILogger<UpdateRoleCommandHandler> logger)
    {
        _roleRepository = roleRepository;
        _logger = logger;
    }

    public async Task<RoleDto> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id);
        if (role == null)
        {
            throw new KeyNotFoundException($"Rol con ID {request.Id} no encontrado");
        }

        // Verificar si el nuevo nombre ya existe en otro rol
        var existingRole = await _roleRepository.GetByNameAsync(request.Name);
        if (existingRole != null && existingRole.Id != request.Id)
        {
            throw new InvalidOperationException($"Ya existe otro rol con el nombre '{request.Name}'");
        }

        role.Name = request.Name;
        role.Description = request.Description;
        role.Permissions = request.Permissions;
        role.IsActive = request.IsActive;
        role.UpdatedAt = DateTime.UtcNow;

        var updatedRole = await _roleRepository.UpdateAsync(role);
        _logger.LogInformation("Rol actualizado: {RoleId} - {RoleName}", updatedRole.Id, updatedRole.Name);

        return new RoleDto
        {
            Id = updatedRole.Id,
            Name = updatedRole.Name,
            Description = updatedRole.Description,
            Permissions = updatedRole.Permissions,
            IsActive = updatedRole.IsActive,
            CreatedAt = updatedRole.CreatedAt,
            UpdatedAt = updatedRole.UpdatedAt
        };
    }
}

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, bool>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IUserRepository _userRepository;
    private readonly ILogger<DeleteRoleCommandHandler> _logger;

    public DeleteRoleCommandHandler(
        IRoleRepository roleRepository,
        IUserRepository userRepository,
        ILogger<DeleteRoleCommandHandler> logger)
    {
        _roleRepository = roleRepository;
        _userRepository = userRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id);
        if (role == null)
        {
            throw new KeyNotFoundException($"Rol con ID {request.Id} no encontrado");
        }

        // Verificar si hay usuarios con este rol
        var usersWithRole = await _userRepository.GetByRoleIdAsync(request.Id);
        if (usersWithRole.Any())
        {
            throw new InvalidOperationException(
                $"No se puede eliminar el rol '{role.Name}' porque tiene {usersWithRole.Count()} usuario(s) asignado(s)");
        }

        await _roleRepository.DeleteAsync(request.Id);
        _logger.LogInformation("Rol eliminado: {RoleId} - {RoleName}", request.Id, role.Name);

        return true;
    }
}

public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, bool>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly ILogger<AssignRoleToUserCommandHandler> _logger;

    public AssignRoleToUserCommandHandler(
        IUserRepository userRepository,
        IRoleRepository roleRepository,
        ILogger<AssignRoleToUserCommandHandler> logger)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _logger = logger;
    }

    public async Task<bool> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new KeyNotFoundException($"Usuario con ID {request.UserId} no encontrado");
        }

        var role = await _roleRepository.GetByIdAsync(request.RoleId);
        if (role == null)
        {
            throw new KeyNotFoundException($"Rol con ID {request.RoleId} no encontrado");
        }

        if (!role.IsActive)
        {
            throw new InvalidOperationException($"No se puede asignar el rol '{role.Name}' porque está inactivo");
        }

        user.RoleId = request.RoleId;
        await _userRepository.UpdateAsync(user);

        _logger.LogInformation(
            "Rol {RoleName} (ID: {RoleId}) asignado al usuario {UserEmail} (ID: {UserId})",
            role.Name, role.Id, user.Email, user.Id);

        return true;
    }
}
