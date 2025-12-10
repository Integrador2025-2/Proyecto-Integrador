using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.Objetivos;

public class CreateObjetivoCommand : IRequest<ObjetivoDto>
{
    public int ProyectoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string ResultadoEsperado { get; set; } = string.Empty;
}

public class UpdateObjetivoCommand : IRequest<ObjetivoDto>
{
    public int ObjetivoId { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public string ResultadoEsperado { get; set; } = string.Empty;
}

public class DeleteObjetivoCommand : IRequest<bool>
{
    public int ObjetivoId { get; set; }
}
