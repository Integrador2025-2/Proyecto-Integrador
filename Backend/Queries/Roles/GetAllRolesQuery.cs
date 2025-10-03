using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.Roles;

public class GetAllRolesQuery : IRequest<List<RoleDto>>
{
    public bool? IsActive { get; set; }
    public string? SearchTerm { get; set; }
}
