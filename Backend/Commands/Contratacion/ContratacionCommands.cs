using Backend.Models.DTOs;
using MediatR;

namespace Backend.Commands.Contratacion;

public record CreateContratacionCommand(
    string NivelGestion,
    string Categoria,
    string IdentidadAcademica,
    string ExperienciaMinima,
    decimal Iva,
    decimal ValorMensual
) : IRequest<ContratacionDto>;

public record UpdateContratacionCommand(
    int ContratacionId,
    string NivelGestion,
    string Categoria,
    string IdentidadAcademica,
    string ExperienciaMinima,
    decimal Iva,
    decimal ValorMensual
) : IRequest<ContratacionDto>;

public record DeleteContratacionCommand(int ContratacionId) : IRequest;
