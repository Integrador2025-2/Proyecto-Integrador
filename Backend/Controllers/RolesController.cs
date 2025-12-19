using Backend.Commands.Roles;
using Backend.Models.DTOs;
using Backend.Queries.Roles;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RolesController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<RolesController> _logger;

    public RolesController(IMediator mediator, ILogger<RolesController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    /// <summary>
    /// Obtiene todos los roles del sistema
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoleDto>>> GetAll()
    {
        try
        {
            var roles = await _mediator.Send(new GetAllRolesQuery());
            return Ok(roles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener roles");
            return StatusCode(500, "Error al obtener los roles");
        }
    }

    /// <summary>
    /// Obtiene un rol por su ID
    /// </summary>
    [HttpGet("{id}")]
    public async Task<ActionResult<RoleDto>> GetById(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("El ID debe ser mayor a 0");
            }

            var role = await _mediator.Send(new GetRoleByIdQuery(id));
            if (role == null)
            {
                return NotFound($"Rol con ID {id} no encontrado");
            }

            return Ok(role);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener rol {RoleId}", id);
            return StatusCode(500, "Error al obtener el rol");
        }
    }

    /// <summary>
    /// Obtiene un rol por su nombre
    /// </summary>
    [HttpGet("nombre/{name}")]
    public async Task<ActionResult<RoleDto>> GetByName(string name)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return BadRequest("El nombre no puede estar vacío");
            }

            var role = await _mediator.Send(new GetRoleByNameQuery(name));
            if (role == null)
            {
                return NotFound($"Rol '{name}' no encontrado");
            }

            return Ok(role);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener rol por nombre {RoleName}", name);
            return StatusCode(500, "Error al obtener el rol");
        }
    }

    /// <summary>
    /// Obtiene todos los usuarios que tienen un rol específico
    /// </summary>
    [HttpGet("{roleId}/usuarios")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetUsersByRoleId(int roleId)
    {
        try
        {
            if (roleId <= 0)
            {
                return BadRequest("El ID del rol debe ser mayor a 0");
            }

            var users = await _mediator.Send(new GetUsersByRoleIdQuery(roleId));
            return Ok(users);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al obtener usuarios del rol {RoleId}", roleId);
            return StatusCode(500, "Error al obtener los usuarios del rol");
        }
    }

    /// <summary>
    /// Crea un nuevo rol
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<RoleDto>> Create([FromBody] CreateRoleDto createRoleDto)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(createRoleDto.Name))
            {
                return BadRequest("El nombre del rol es requerido");
            }

            var command = new CreateRoleCommand(
                createRoleDto.Name,
                createRoleDto.Description,
                createRoleDto.Permissions
            );

            var role = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetById), new { id = role.Id }, role);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Error al crear rol: {Message}", ex.Message);
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al crear rol");
            return StatusCode(500, "Error al crear el rol");
        }
    }

    /// <summary>
    /// Actualiza un rol existente
    /// </summary>
    [HttpPut("{id}")]
    public async Task<ActionResult<RoleDto>> Update(int id, [FromBody] UpdateRoleDto updateRoleDto)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("El ID debe ser mayor a 0");
            }

            if (id != updateRoleDto.Id)
            {
                return BadRequest("El ID de la URL no coincide con el ID del cuerpo");
            }

            if (string.IsNullOrWhiteSpace(updateRoleDto.Name))
            {
                return BadRequest("El nombre del rol es requerido");
            }

            var command = new UpdateRoleCommand(
                updateRoleDto.Id,
                updateRoleDto.Name,
                updateRoleDto.Description,
                updateRoleDto.Permissions,
                updateRoleDto.IsActive
            );

            var role = await _mediator.Send(command);
            return Ok(role);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Rol no encontrado: {Message}", ex.Message);
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Error al actualizar rol: {Message}", ex.Message);
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al actualizar rol");
            return StatusCode(500, "Error al actualizar el rol");
        }
    }

    /// <summary>
    /// Elimina un rol (solo si no tiene usuarios asignados)
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        try
        {
            if (id <= 0)
            {
                return BadRequest("El ID debe ser mayor a 0");
            }

            var result = await _mediator.Send(new DeleteRoleCommand(id));
            if (result)
            {
                return NoContent();
            }

            return NotFound($"Rol con ID {id} no encontrado");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Rol no encontrado: {Message}", ex.Message);
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "No se puede eliminar el rol: {Message}", ex.Message);
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al eliminar rol");
            return StatusCode(500, "Error al eliminar el rol");
        }
    }

    /// <summary>
    /// Asigna un rol a un usuario
    /// </summary>
    [HttpPost("asignar")]
    public async Task<ActionResult> AssignRoleToUser([FromBody] AssignRoleToUserDto assignDto)
    {
        try
        {
            if (assignDto.UserId <= 0)
            {
                return BadRequest("El ID del usuario debe ser mayor a 0");
            }

            if (assignDto.RoleId <= 0)
            {
                return BadRequest("El ID del rol debe ser mayor a 0");
            }

            var command = new AssignRoleToUserCommand(assignDto.UserId, assignDto.RoleId);
            var result = await _mediator.Send(command);

            if (result)
            {
                return Ok(new { message = "Rol asignado exitosamente" });
            }

            return BadRequest("No se pudo asignar el rol");
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Entidad no encontrada: {Message}", ex.Message);
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            _logger.LogWarning(ex, "Error al asignar rol: {Message}", ex.Message);
            return Conflict(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error inesperado al asignar rol");
            return StatusCode(500, "Error al asignar el rol");
        }
    }
}
