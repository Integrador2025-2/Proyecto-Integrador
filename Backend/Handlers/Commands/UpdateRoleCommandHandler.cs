using AutoMapper;
using Backend.Commands.Roles;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using MediatR;

namespace Backend.Handlers.Commands;

public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, Backend.Models.DTOs.RoleDto>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public UpdateRoleCommandHandler(IRoleRepository roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<Backend.Models.DTOs.RoleDto> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id);
        if (role == null)
        {
            throw new ArgumentException($"Rol con ID {request.Id} no encontrado.");
        }

        // Verificar si el nombre ya existe (excluyendo el rol actual)
        if (await _roleRepository.NameExistsAsync(request.Name, request.Id))
        {
            throw new ArgumentException($"Ya existe un rol con el nombre '{request.Name}'.");
        }

        role.Name = request.Name;
        role.Description = request.Description;
        role.Permissions = request.Permissions;
        role.IsActive = request.IsActive;

        var updatedRole = await _roleRepository.UpdateAsync(role);
        return _mapper.Map<Backend.Models.DTOs.RoleDto>(updatedRole);
    }
}
