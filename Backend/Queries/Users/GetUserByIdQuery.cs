using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.Users;

public class GetUserByIdQuery : IRequest<UserDto?>
{
    public int Id { get; set; }
}