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
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.RoleId, opt => opt.MapFrom(src => src.RoleId))
            .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role != null ? src.Role.Name : null));

        CreateMap<CreateUserDto, User>();
        CreateMap<UpdateUserDto, User>();
        
        // Role mappings
        CreateMap<Role, RoleDto>();
        
        // Rubro mappings
        CreateMap<Rubro, RubroDto>();
        CreateMap<CreateRubroDto, Rubro>();
        CreateMap<UpdateRubroDto, Rubro>();
        
        // RecursoEspecifico mappings
        CreateMap<RecursoEspecifico, RecursoEspecificoDto>();
        CreateMap<CreateRecursoEspecificoDto, RecursoEspecifico>();
        CreateMap<UpdateRecursoEspecificoDto, RecursoEspecifico>();
        
        // Contratacion mappings
        CreateMap<Contratacion, ContratacionDto>();
        CreateMap<CreateContratacionDto, Contratacion>();
        CreateMap<UpdateContratacionDto, Contratacion>();
        
        // TalentoHumano mappings
        CreateMap<TalentoHumano, TalentoHumanoDto>();
        CreateMap<CreateTalentoHumanoDto, TalentoHumano>();
        CreateMap<UpdateTalentoHumanoDto, TalentoHumano>();
        
        // TalentoHumanoTarea mappings
        CreateMap<TalentoHumanoTarea, TalentoHumanoTareaDto>();
        CreateMap<CreateTalentoHumanoTareaDto, TalentoHumanoTarea>();
        CreateMap<UpdateTalentoHumanoTareaDto, TalentoHumanoTarea>();
        
        // RemuneracionPorAnio mappings
        CreateMap<RemuneracionPorAnio, RemuneracionPorAnioDto>();
        CreateMap<CreateRemuneracionPorAnioDto, RemuneracionPorAnio>();
        CreateMap<UpdateRemuneracionPorAnioDto, RemuneracionPorAnio>();
        
        // Administrativos mappings
        CreateMap<Administrativos, AdministrativosDto>();
        CreateMap<CreateAdministrativosDto, Administrativos>();
        CreateMap<UpdateAdministrativosDto, Administrativos>();
        
        // ProteccionConocimientoDivulgacion mappings
        CreateMap<ProteccionConocimientoDivulgacion, ProteccionConocimientoDivulgacionDto>();
        CreateMap<CreateProteccionConocimientoDivulgacionDto, ProteccionConocimientoDivulgacion>();
        CreateMap<UpdateProteccionConocimientoDivulgacionDto, ProteccionConocimientoDivulgacion>();
        
        // SeguimientoEvaluacion mappings
        CreateMap<SeguimientoEvaluacion, SeguimientoEvaluacionDto>();
        CreateMap<CreateSeguimientoEvaluacionDto, SeguimientoEvaluacion>();
        CreateMap<UpdateSeguimientoEvaluacionDto, SeguimientoEvaluacion>();
        
        // Divulgacion mappings
        CreateMap<Divulgacion, DivulgacionDto>();
        CreateMap<CreateDivulgacionDto, Divulgacion>();
        CreateMap<UpdateDivulgacionDto, Divulgacion>();
        
        // Otros mappings
        CreateMap<Otros, OtrosDto>();
        CreateMap<CreateOtrosDto, Otros>();
        CreateMap<UpdateOtrosDto, Otros>();
        
        // EquiposSoftware mappings
        CreateMap<EquiposSoftware, EquiposSoftwareDto>();
        CreateMap<CreateEquiposSoftwareDto, EquiposSoftware>();
        CreateMap<UpdateEquiposSoftwareDto, EquiposSoftware>();
        
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
        
        // Infraestructura mappings
        CreateMap<Infraestructura, InfraestructuraDto>();
        CreateMap<CreateInfraestructuraDto, Infraestructura>();
        CreateMap<UpdateInfraestructuraDto, Infraestructura>();
    }
}
