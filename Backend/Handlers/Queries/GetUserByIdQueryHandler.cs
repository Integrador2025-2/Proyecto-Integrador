using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Users;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetUserByIdQueryHandler : IRequestHandler<GetUserByIdQuery, UserDto?>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUserByIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<UserDto?> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        return user == null ? null : _mapper.Map<UserDto>(user);
    }
}


