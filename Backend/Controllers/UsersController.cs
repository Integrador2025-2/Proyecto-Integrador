using Backend.Commands.Users;
using Backend.Models.DTOs;
using Backend.Queries.Users;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los usuarios
    /// </summary>
    /// <param name="isActive">Filtrar por estado activo</param>
    /// <param name="searchTerm">Término de búsqueda</param>
    /// <returns>Lista de usuarios</returns>
    [HttpGet]
    public async Task<ActionResult<List<UserDto>>> GetAllUsers(
        [FromQuery] bool? isActive = null,
        [FromQuery] string? searchTerm = null)
    {
        var query = new GetAllUsersQuery
        {
            IsActive = isActive,
            SearchTerm = searchTerm
        };

        var users = await _mediator.Send(query);
        return Ok(users);
    }

    /// <summary>
    /// Obtiene un usuario por ID
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <returns>Usuario encontrado</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(int id)
    {
        var query = new GetUserByIdQuery { Id = id };
        var user = await _mediator.Send(query);

        if (user == null)
        {
            return NotFound($"Usuario con ID {id} no encontrado.");
        }

        return Ok(user);
    }

    /// <summary>
    /// Crea un nuevo usuario
    /// </summary>
    /// <param name="createUserDto">Datos del usuario a crear</param>
    /// <returns>Usuario creado</returns>
    [HttpPost]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] CreateUserDto createUserDto)
    {
        var command = new CreateUserCommand
        {
            FirstName = createUserDto.FirstName,
            LastName = createUserDto.LastName,
            Email = createUserDto.Email,
            Password = createUserDto.Password
        };

        var user = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetUserById), new { id = user.Id }, user);
    }

    /// <summary>
    /// Actualiza un usuario existente
    /// </summary>
    /// <param name="id">ID del usuario</param>
    /// <param name="updateUserDto">Datos actualizados del usuario</param>
    /// <returns>Usuario actualizado</returns>
    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> UpdateUser(int id, [FromBody] UpdateUserDto updateUserDto)
    {
        if (id != updateUserDto.Id)
        {
            return BadRequest("El ID en la URL no coincide con el ID en el cuerpo de la petición.");
        }

        var command = new UpdateUserCommand
        {
            Id = updateUserDto.Id,
            FirstName = updateUserDto.FirstName,
            LastName = updateUserDto.LastName,
            Email = updateUserDto.Email,
            IsActive = updateUserDto.IsActive
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
    }

    /// <summary>
    /// Elimina un usuario
    /// </summary>
    /// <param name="id">ID del usuario a eliminar</param>
    /// <returns>Resultado de la eliminación</returns>
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteUser(int id)
    {
        var command = new DeleteUserCommand { Id = id };

        try
        {
            var result = await _mediator.Send(command);
            if (result)
            {
                return NoContent();
            }
            return NotFound($"Usuario con ID {id} no encontrado.");
        }
        catch (ArgumentException ex)
        {
            return NotFound(ex.Message);
        }
    }
}




