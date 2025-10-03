using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.Weather;

public class GetWeatherForecastByIdQuery : IRequest<WeatherForecastDto?>
{
    public int Id { get; set; }
}





