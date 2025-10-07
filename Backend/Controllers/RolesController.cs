using Backend.Commands.Roles;
using Backend.Models.DTOs;
using Backend.Queries.Roles;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;

    public RolesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los roles
    /// </summary>
    /// <param name="isActive">Filtrar por estado activo</param>
    /// <param name="searchTerm">Término de búsqueda</param>
    /// <returns>Lista de roles</returns>
    [HttpGet]
    public async Task<ActionResult<List<RoleDto>>> GetAllRoles(
        [FromQuery] bool? isActive = null,
        [FromQuery] string? searchTerm = null)
    {
        var query = new GetAllRolesQuery
        {
            IsActive = isActive,
            SearchTerm = searchTerm
        };

        var roles = await _mediator.Send(query);
        return Ok(roles);
    }

    /// <summary>
    /// Obtiene un rol por ID
    /// </summary>
    /// <param name="id">ID del rol</param>
    /// <returns>Rol encontrado</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDto>> GetRoleById(int id)
    {
        var query = new GetRoleByIdQuery { Id = id };
        var role = await _mediator.Send(query);

        if (role == null)
        {
            return NotFound($"Rol con ID {id} no encontrado.");
        }

        return Ok(role);
    }

    /// <summary>
    /// Crea un nuevo rol
    /// </summary>
    /// <param name="createRoleDto">Datos del rol a crear</param>
    /// <returns>Rol creado</returns>
    [HttpPost]
    public async Task<ActionResult<RoleDto>> CreateRole([FromBody] CreateRoleDto createRoleDto)
    {
        var command = new CreateRoleCommand
        {
            Name = createRoleDto.Name,
            Description = createRoleDto.Description,
            Permissions = createRoleDto.Permissions
        };

        try
        {
            var role = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetRoleById), new { id = role.Id }, role);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Actualiza un rol existente
    /// </summary>
    /// <param name="id">ID del rol</param>
    /// <param name="updateRoleDto">Datos actualizados del rol</param>
    /// <returns>Rol actualizado</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<RoleDto>> UpdateRole(int id, [FromBody] UpdateRoleDto updateRoleDto)
    {
        if (id != updateRoleDto.Id)
        {
            return BadRequest("El ID en la URL no coincide con el ID en el cuerpo de la petición.");
        }

        var command = new UpdateRoleCommand
        {
            Id = updateRoleDto.Id,
            Name = updateRoleDto.Name,
            Description = updateRoleDto.Description,
            Permissions = updateRoleDto.Permissions,
            IsActive = updateRoleDto.IsActive
        };

        try
        {
            var role = await _mediator.Send(command);
            return Ok(role);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }

    /// <summary>
    /// Elimina un rol
    /// </summary>
    /// <param name="id">ID del rol a eliminar</param>
    /// <returns>Resultado de la eliminación</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRole(int id)
    {
        var command = new DeleteRoleCommand { Id = id };

        try
        {
            var result = await _mediator.Send(command);
            if (result)
            {
                return NoContent();
            }
            return NotFound($"Rol con ID {id} no encontrado.");
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    /// <summary>
    /// Obtiene todos los usuarios de un rol específico
    /// </summary>
    /// <param name="id">ID del rol</param>
    /// <returns>Lista de usuarios del rol</returns>
    [HttpGet("{id}/users")]
    public async Task<ActionResult<List<UserDto>>> GetUsersByRole(int id)
    {
        var query = new GetUsersByRoleQuery { RoleId = id };
        var users = await _mediator.Send(query);
        return Ok(users);
    }

    /// <summary>
    /// Asigna un rol a un usuario
    /// </summary>
    /// <param name="assignRoleDto">Datos de asignación de rol</param>
    /// <returns>Usuario actualizado</returns>
    [HttpPost("assign")]
    public async Task<ActionResult<UserDto>> AssignRoleToUser([FromBody] AssignRoleToUserDto assignRoleDto)
    {
        var command = new AssignRoleToUserCommand
        {
            UserId = assignRoleDto.UserId,
            RoleId = assignRoleDto.RoleId
        };

        try
        {
            var user = await _mediator.Send(command);
            return Ok(user);
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
