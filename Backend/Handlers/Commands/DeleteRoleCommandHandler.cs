using Backend.Commands.Roles;
using Backend.Infrastructure.Repositories;
using MediatR;

namespace Backend.Handlers.Commands;

public class DeleteRoleCommandHandler : IRequestHandler<DeleteRoleCommand, bool>
{
    private readonly IRoleRepository _roleRepository;

    public DeleteRoleCommandHandler(IRoleRepository roleRepository)
    {
        _roleRepository = roleRepository;
    }

    public async Task<bool> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        var role = await _roleRepository.GetByIdAsync(request.Id);
        if (role == null)
        {
            throw new ArgumentException($"Rol con ID {request.Id} no encontrado.");
        }

        // Verificar si el rol tiene usuarios asignados
        // TODO: Implementar verificación de usuarios asignados
        // if (await _userRepository.HasUsersWithRoleAsync(request.Id))
        // {
        //     throw new InvalidOperationException("No se puede eliminar un rol que tiene usuarios asignados.");
        // }

        return await _roleRepository.DeleteAsync(request.Id);
    }
}
