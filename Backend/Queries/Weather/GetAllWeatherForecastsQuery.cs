using Backend.Models.DTOs;
using MediatR;

namespace Backend.Queries.Weather;

public class GetAllWeatherForecastsQuery : IRequest<List<WeatherForecastDto>>
{
    public DateOnly? FromDate { get; set; }
    public DateOnly? ToDate { get; set; }
    public string? Summary { get; set; }
}





