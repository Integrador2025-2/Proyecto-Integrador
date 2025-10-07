using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.Roles;

public class GetUsersByRoleQuery : IRequest<List<UserDto>>
{
    public int RoleId { get; set; }
}
