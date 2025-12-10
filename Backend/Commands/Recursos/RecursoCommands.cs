using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.Recursos;

public class CreateRecursoCommand : IRequest<RecursoDto>
{
    public int ActividadId { get; set; }
    public int EntidadId { get; set; }
    public int RubroId { get; set; }
    public string TipoRecurso { get; set; } = string.Empty;
    public decimal MontoEfectivo { get; set; }
    public decimal MontoEspecie { get; set; }
    public string Descripcion { get; set; } = string.Empty;
}

public class UpdateRecursoCommand : IRequest<RecursoDto>
{
    public int RecursoId { get; set; }
    public int EntidadId { get; set; }
    public int RubroId { get; set; }
    public string TipoRecurso { get; set; } = string.Empty;
    public decimal MontoEfectivo { get; set; }
    public decimal MontoEspecie { get; set; }
    public string Descripcion { get; set; } = string.Empty;
}

public class DeleteRecursoCommand : IRequest<bool>
{
    public int RecursoId { get; set; }
}
