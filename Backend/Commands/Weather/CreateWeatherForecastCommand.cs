using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.Weather;

public class CreateWeatherForecastCommand : IRequest<WeatherForecastDto>
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
}


