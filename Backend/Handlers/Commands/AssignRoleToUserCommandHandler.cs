using AutoMapper;
using Backend.Commands.Roles;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using MediatR;

namespace Backend.Handlers.Commands;

public class AssignRoleToUserCommandHandler : IRequestHandler<AssignRoleToUserCommand, Backend.Models.DTOs.UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public AssignRoleToUserCommandHandler(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<Backend.Models.DTOs.UserDto> Handle(AssignRoleToUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new ArgumentException($"Usuario con ID {request.UserId} no encontrado.");
        }

        var role = await _roleRepository.GetByIdAsync(request.RoleId);
        if (role == null)
        {
            throw new ArgumentException($"Rol con ID {request.RoleId} no encontrado.");
        }

        if (!role.IsActive)
        {
            throw new InvalidOperationException("No se puede asignar un rol inactivo.");
        }

        user.RoleId = request.RoleId;
        var updatedUser = await _userRepository.UpdateAsync(user);
        
        // Recargar el usuario con el rol para devolver la información completa
        var userWithRole = await _userRepository.GetByIdAsync(updatedUser.Id);
        return _mapper.Map<Backend.Models.DTOs.UserDto>(userWithRole!);
    }
}
