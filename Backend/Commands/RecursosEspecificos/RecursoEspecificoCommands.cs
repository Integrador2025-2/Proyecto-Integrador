using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.RecursosEspecificos;

public class CreateRecursoEspecificoCommand : IRequest<RecursoEspecificoDto>
{
    public int RecursoId { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Detalle { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Total { get; set; }
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}

public class UpdateRecursoEspecificoCommand : IRequest<RecursoEspecificoDto>
{
    public int RecursoEspecificoId { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Detalle { get; set; } = string.Empty;
    public int Cantidad { get; set; }
    public decimal Total { get; set; }
    public int PeriodoNum { get; set; }
    public string PeriodoTipo { get; set; } = string.Empty;
}

public class DeleteRecursoEspecificoCommand : IRequest<bool>
{
    public int RecursoEspecificoId { get; set; }
}
