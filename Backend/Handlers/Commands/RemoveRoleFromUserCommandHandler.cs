using AutoMapper;
using Backend.Commands.Users;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using MediatR;

namespace Backend.Handlers.Commands;

public class RemoveRoleFromUserCommandHandler : IRequestHandler<RemoveRoleFromUserCommand, Backend.Models.DTOs.UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public RemoveRoleFromUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Backend.Models.DTOs.UserDto> Handle(RemoveRoleFromUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId);
        if (user == null)
        {
            throw new ArgumentException($"Usuario con ID {request.UserId} no encontrado.");
        }

        // Asignar rol por defecto (Usuario = 2)
        user.RoleId = 2;
        var updatedUser = await _userRepository.UpdateAsync(user);
        
        // Recargar el usuario con el rol para devolver la información completa
        var userWithRole = await _userRepository.GetByIdAsync(updatedUser.Id);
        return _mapper.Map<Backend.Models.DTOs.UserDto>(userWithRole!);
    }
}
