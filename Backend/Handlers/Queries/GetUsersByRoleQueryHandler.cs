using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Roles;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetUsersByRoleQueryHandler : IRequestHandler<GetUsersByRoleQuery, List<UserDto>>
{
    private readonly IUserRepository _userRepository;
    private readonly IMapper _mapper;

    public GetUsersByRoleQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<List<UserDto>> Handle(GetUsersByRoleQuery request, CancellationToken cancellationToken)
    {
        // Obtener todos los usuarios y filtrar por rol
        var allUsers = await _userRepository.GetAllAsync();
        var usersWithRole = allUsers.Where(u => u.RoleId == request.RoleId).ToList();
        
        return _mapper.Map<List<UserDto>>(usersWithRole);
    }
}
