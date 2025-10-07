using AutoMapper;
using Backend.Models.Domain;
using Backend.Models.DTOs;

namespace Backend.Infrastructure;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        // Role mappings
        CreateMap<Role, RoleDto>();
        CreateMap<CreateRoleDto, Role>();
        CreateMap<UpdateRoleDto, Role>();
        
        // User mappings
        CreateMap<User, UserDto>()
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : string.Empty))
            .ForMember(dest => dest.Provider, opt => opt.MapFrom(src => src.Provider ?? "local"))
            .ForMember(dest => dest.ProfilePictureUrl, opt => opt.MapFrom(src => src.ProfilePictureUrl))
            .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role));
        
        CreateMap<CreateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => 2)); // Por defecto Usuario
        
        CreateMap<UpdateUserDto, User>()
            .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
            .ForMember(dest => dest.CreatedAt, opt => opt.Ignore());

        // WeatherForecast mappings
        CreateMap<WeatherForecast, WeatherForecastDto>();
        CreateMap<CreateWeatherForecastDto, WeatherForecast>();
        CreateMap<UpdateWeatherForecastDto, WeatherForecast>();
    }
}




