using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.Users;

public class GetAllUsersQuery : IRequest<List<UserDto>>
{
    public bool? IsActive { get; set; }
    public string? SearchTerm { get; set; }
}



