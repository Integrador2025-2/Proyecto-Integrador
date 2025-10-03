using AutoMapper;
using Backend.Commands.Users;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using MediatR;

namespace Backend.Handlers.Commands;

public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, Backend.Models.DTOs.UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public UpdateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Backend.Models.DTOs.UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _userRepository.GetByIdAsync(request.Id);
        if (existingUser == null)
        {
            throw new ArgumentException($"Usuario con ID {request.Id} no encontrado.");
        }

        existingUser.FirstName = request.FirstName;
        existingUser.LastName = request.LastName;
        existingUser.Email = request.Email;
        existingUser.IsActive = request.IsActive;

        var updatedUser = await _userRepository.UpdateAsync(existingUser);
        return _mapper.Map<Backend.Models.DTOs.UserDto>(updatedUser);
    }
}





