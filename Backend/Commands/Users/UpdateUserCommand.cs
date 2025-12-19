using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.Users;

public class UpdateUserCommand : IRequest<UserDto>
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public bool IsActive { get; set; }
}


