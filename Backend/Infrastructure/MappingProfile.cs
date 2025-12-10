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
        CreateMap<Proyecto, ProyectoDto>();
        
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
        CreateMap<CreateProyectoDto, Proyecto>();
        CreateMap<UpdateProyectoDto, Proyecto>();
        
        // Actividad mappings
        CreateMap<Actividad, ActividadDto>()
            .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.ValorTotal))
            .ForMember(dest => dest.TotalxAnios, opt => opt.MapFrom(src => src.TotalxAnios))
            .ForMember(dest => dest.ValorUnitario, opt => opt.MapFrom(src => src.ValorUnitario));

        // Map various rubro collections into a unified Rubros list
        CreateMap<Actividad, ActividadDto>()
            .ForMember(dest => dest.Rubros, opt => opt.MapFrom(src =>
                (src.TalentoHumano != null ? src.TalentoHumano.Select(t => new RubroItemDto {
                    Tipo = "TalentoHumano",
                    Id = t.TalentoHumanoId,
                    Descripcion = t.CargoEspecifico,
                    Total = t.Total,
                    PeriodoTipo = t.PeriodoTipo,
                    PeriodoNum = t.PeriodoNum,
                    CargoEspecifico = t.CargoEspecifico,
                    Semanas = t.Semanas
                }) : Enumerable.Empty<RubroItemDto>())
                .Concat(src.EquiposSoftware != null ? src.EquiposSoftware.Select(e => new RubroItemDto {
                    Tipo = "EquiposSoftware",
                    Id = e.EquiposSoftwareId,
                    Descripcion = e.EspecificacionesTecnicas,
                    Total = e.Total,
                    PeriodoTipo = e.PeriodoTipo,
                    PeriodoNum = e.PeriodoNum,
                    Cantidad = e.Cantidad
                }) : Enumerable.Empty<RubroItemDto>())
                .Concat(src.ServiciosTecnologicos != null ? src.ServiciosTecnologicos.Select(s => new RubroItemDto {
                    Tipo = "ServiciosTecnologicos",
                    Id = s.ServiciosTecnologicosId,
                    Descripcion = s.Descripcion,
                    Total = s.Total,
                    PeriodoTipo = s.PeriodoTipo,
                    PeriodoNum = s.PeriodoNum
                }) : Enumerable.Empty<RubroItemDto>())
                .Concat(src.MaterialesInsumos != null ? src.MaterialesInsumos.Select(m => new RubroItemDto {
                    Tipo = "MaterialesInsumos",
                    Id = m.MaterialesInsumosId,
                    Descripcion = m.Materiales,
                    Total = m.Total,
                    PeriodoTipo = m.PeriodoTipo,
                    PeriodoNum = m.PeriodoNum
                }) : Enumerable.Empty<RubroItemDto>())
                .Concat(src.CapacitacionEventos != null ? src.CapacitacionEventos.Select(c => new RubroItemDto {
                    Tipo = "CapacitacionEventos",
                    Id = c.CapacitacionEventosId,
                    Descripcion = c.Tema,
                    Total = c.Total,
                    PeriodoTipo = c.PeriodoTipo,
                    PeriodoNum = c.PeriodoNum,
                    Cantidad = c.Cantidad
                }) : Enumerable.Empty<RubroItemDto>())
                .Concat(src.GastosViaje != null ? src.GastosViaje.Select(g => new RubroItemDto {
                    Tipo = "GastosViaje",
                    Id = g.GastosViajeId,
                    Descripcion = string.Empty,
                    Total = g.Costo,
                    PeriodoTipo = g.PeriodoTipo,
                    PeriodoNum = g.PeriodoNum
                }) : Enumerable.Empty<RubroItemDto>())
            ));

        CreateMap<CreateActividadDto, Actividad>()
            .ForMember(dest => dest.TotalxAnios, opt => opt.MapFrom(src => src.TotalxAnios))
            .ForMember(dest => dest.ValorUnitario, opt => opt.MapFrom(src => src.ValorUnitario));

        CreateMap<UpdateActividadDto, Actividad>()
            .ForMember(dest => dest.TotalxAnios, opt => opt.MapFrom(src => src.TotalxAnios))
            .ForMember(dest => dest.ValorUnitario, opt => opt.MapFrom(src => src.ValorUnitario));
        
        // Rubro mappings
        CreateMap<Rubro, RubroDto>();
        CreateMap<CreateRubroDto, Rubro>();
        CreateMap<UpdateRubroDto, Rubro>();

        // TalentoHumano mappings
        CreateMap<TalentoHumano, TalentoHumanoDto>();
        CreateMap<CreateTalentoHumanoDto, TalentoHumano>();
        CreateMap<UpdateTalentoHumanoDto, TalentoHumano>();
                
        // ServiciosTecnologicos mappings
        CreateMap<ServiciosTecnologicos, ServiciosTecnologicosDto>();
        CreateMap<CreateServiciosTecnologicosDto, ServiciosTecnologicos>();
        CreateMap<UpdateServiciosTecnologicosDto, ServiciosTecnologicos>();
        
        // MaterialesInsumos mappings
        CreateMap<MaterialesInsumos, MaterialesInsumosDto>();
        CreateMap<CreateMaterialesInsumosDto, MaterialesInsumos>();
        CreateMap<UpdateMaterialesInsumosDto, MaterialesInsumos>();

        // CapacitacionEventos mappings
        CreateMap<CapacitacionEventos, CapacitacionEventosDto>();
        CreateMap<CreateCapacitacionEventosDto, CapacitacionEventos>();
        CreateMap<UpdateCapacitacionEventosDto, CapacitacionEventos>();

        // GastosViaje mappings
        CreateMap<GastosViaje, GastosViajeDto>();
        CreateMap<CreateGastosViajeDto, GastosViaje>();
        CreateMap<UpdateGastosViajeDto, GastosViaje>();

        // EquiposSoftware mappings
        CreateMap<EquiposSoftware, EquiposSoftwareDto>();
        CreateMap<CreateEquiposSoftwareDto, EquiposSoftware>();
        CreateMap<UpdateEquiposSoftwareDto, EquiposSoftware>();

        // Entidad mappings
        CreateMap<Entidad, EntidadDto>();
        CreateMap<CreateEntidadDto, Entidad>();
        CreateMap<UpdateEntidadDto, Entidad>();

        // ActXEntidad mappings
        CreateMap<ActXEntidad, ActxEntidadDto>()
            .ForMember(dest => dest.Entidad, opt => opt.MapFrom(src => src.Entidad));
        CreateMap<CreateActxEntidadDto, ActXEntidad>();
        CreateMap<UpdateActxEntidadDto, ActXEntidad>();

        // CadenaDeValor mappings
        CreateMap<CadenaDeValor, CadenaDeValorDto>();
        CreateMap<CreateCadenaDeValorDto, CadenaDeValor>();
        CreateMap<UpdateCadenaDeValorDto, CadenaDeValor>();

        // Tarea mappings
        CreateMap<Tarea, TareaDto>();
        CreateMap<CreateTareaDto, Tarea>();
        CreateMap<UpdateTareaDto, Tarea>();
    }
}




