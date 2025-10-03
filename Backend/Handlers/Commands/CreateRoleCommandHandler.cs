using AutoMapper;
using Backend.Commands.Roles;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateRoleCommandHandler : IRequestHandler<CreateRoleCommand, Backend.Models.DTOs.RoleDto>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public CreateRoleCommandHandler(IRoleRepository roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<Backend.Models.DTOs.RoleDto> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        // Verificar si el nombre ya existe
        if (await _roleRepository.NameExistsAsync(request.Name))
        {
            throw new ArgumentException($"Ya existe un rol con el nombre '{request.Name}'.");
        }

        var role = new Role
        {
            Name = request.Name,
            Description = request.Description,
            Permissions = request.Permissions,
            IsActive = true
        };

        var createdRole = await _roleRepository.CreateAsync(role);
        return _mapper.Map<Backend.Models.DTOs.RoleDto>(createdRole);
    }
}
