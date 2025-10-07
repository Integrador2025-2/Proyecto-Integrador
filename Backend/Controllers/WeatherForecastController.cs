using Backend.Commands.Weather;
using Backend.Models.DTOs;
using Backend.Queries.Weather;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WeatherForecastController : ControllerBase
{
    private readonly IMediator _mediator;

    public WeatherForecastController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Obtiene todos los pronósticos del clima
    /// </summary>
    /// <param name="fromDate">Fecha desde</param>
    /// <param name="toDate">Fecha hasta</param>
    /// <param name="summary">Filtrar por resumen</param>
    /// <returns>Lista de pronósticos del clima</returns>
    [HttpGet]
    public async Task<ActionResult<List<WeatherForecastDto>>> GetAllWeatherForecasts(
        [FromQuery] DateOnly? fromDate = null,
        [FromQuery] DateOnly? toDate = null,
        [FromQuery] string? summary = null)
    {
        var query = new GetAllWeatherForecastsQuery
        {
            FromDate = fromDate,
            ToDate = toDate,
            Summary = summary
        };

        var forecasts = await _mediator.Send(query);
        return Ok(forecasts);
    }

    /// <summary>
    /// Obtiene un pronóstico específico por ID
    /// </summary>
    /// <param name="id">ID del pronóstico</param>
    /// <returns>Pronóstico específico</returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<WeatherForecastDto>> GetWeatherForecastById(int id)
    {
        var query = new GetWeatherForecastByIdQuery { Id = id };
        var forecast = await _mediator.Send(query);

        if (forecast == null)
        {
            return NotFound($"Pronóstico con ID {id} no encontrado.");
        }

        return Ok(forecast);
    }

    /// <summary>
    /// Crea un nuevo pronóstico del clima
    /// </summary>
    /// <param name="createForecastDto">Datos del pronóstico a crear</param>
    /// <returns>Pronóstico creado</returns>
    [HttpPost]
    public async Task<ActionResult<WeatherForecastDto>> CreateWeatherForecast([FromBody] CreateWeatherForecastDto createForecastDto)
    {
        var command = new CreateWeatherForecastCommand
        {
            Date = createForecastDto.Date,
            TemperatureC = createForecastDto.TemperatureC,
            Summary = createForecastDto.Summary
        };

        var forecast = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetWeatherForecastById), new { id = forecast.Id }, forecast);
    }
}
