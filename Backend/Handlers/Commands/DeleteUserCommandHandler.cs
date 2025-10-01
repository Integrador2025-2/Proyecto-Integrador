using Backend.Commands.Users;
using Backend.Infrastructure.Repositories;
using MediatR;

namespace Backend.Handlers.Commands;

public class DeleteUserCommandHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserCommandHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var exists = await _userRepository.ExistsAsync(request.Id);
        if (!exists)
        {
            throw new ArgumentException($"Usuario con ID {request.Id} no encontrado.");
        }

        return await _userRepository.DeleteAsync(request.Id);
    }
}


