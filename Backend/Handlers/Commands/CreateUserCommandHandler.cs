using AutoMapper;
using Backend.Commands.Users;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, Backend.Models.DTOs.UserDto>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public CreateUserCommandHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Backend.Models.DTOs.UserDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = new User
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            Email = request.Email,
            IsActive = true
        };

        var createdUser = await _userRepository.CreateAsync(user);
        return _mapper.Map<Backend.Models.DTOs.UserDto>(createdUser);
    }
}



