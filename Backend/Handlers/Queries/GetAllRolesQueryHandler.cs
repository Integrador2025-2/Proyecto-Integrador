using AutoMapper;
using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Roles;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetAllRolesQueryHandler : IRequestHandler<GetAllRolesQuery, List<RoleDto>>
{
    private readonly IRoleRepository _roleRepository;
    private readonly IMapper _mapper;

    public GetAllRolesQueryHandler(IRoleRepository roleRepository, IMapper mapper)
    {
        _roleRepository = roleRepository;
        _mapper = mapper;
    }

    public async Task<List<RoleDto>> Handle(GetAllRolesQuery request, CancellationToken cancellationToken)
    {
        var roles = await _roleRepository.GetByFilterAsync(request.IsActive, request.SearchTerm);
        return _mapper.Map<List<RoleDto>>(roles);
    }
}
