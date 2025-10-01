using AutoMapper;
using Backend.Models.Domain;
using Backend.Models.DTOs;

namespace Backend.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName));
        
        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();

        // WeatherForecast mappings
        CreateMap<WeatherForecast, WeatherForecastDto>();
        CreateMap<CreateWeatherForecastDto, WeatherForecast>();
        CreateMap<UpdateWeatherForecastDto, WeatherForecast>();
    }
}


