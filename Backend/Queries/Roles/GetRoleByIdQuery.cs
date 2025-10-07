using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.Roles;

public class GetRoleByIdQuery : IRequest<RoleDto?>
{
    public int Id { get; set; }
}
