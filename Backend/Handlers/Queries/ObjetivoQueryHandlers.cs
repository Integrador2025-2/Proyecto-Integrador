using Backend.Infrastructure.Repositories;
using Backend.Models.DTOs;
using Backend.Queries.Objetivos;
using MediatR;

namespace Backend.Handlers.Queries;

public class GetObjetivoByIdHandler : IRequestHandler<GetObjetivoByIdQuery, ObjetivoDto?>
{
    private readonly IObjetivoRepository _objetivoRepository;

    public GetObjetivoByIdHandler(IObjetivoRepository objetivoRepository)
    {
        _objetivoRepository = objetivoRepository;
    }

    public async Task<ObjetivoDto?> Handle(GetObjetivoByIdQuery request, CancellationToken cancellationToken)
    {
        var objetivo = await _objetivoRepository.GetByIdAsync(request.ObjetivoId);
        if (objetivo == null)
            return null;

        return new ObjetivoDto
        {
            ObjetivoId = objetivo.ObjetivoId,
            ProyectoId = objetivo.ProyectoId,
            Nombre = objetivo.Nombre,
            Descripcion = objetivo.Descripcion,
            ResultadoEsperado = objetivo.ResultadoEsperado,
            ProyectoNombre = objetivo.Proyecto?.Nombre
        };
    }
}

public class GetAllObjetivosHandler : IRequestHandler<GetAllObjetivosQuery, IEnumerable<ObjetivoDto>>
{
    private readonly IObjetivoRepository _objetivoRepository;

    public GetAllObjetivosHandler(IObjetivoRepository objetivoRepository)
    {
        _objetivoRepository = objetivoRepository;
    }

    public async Task<IEnumerable<ObjetivoDto>> Handle(GetAllObjetivosQuery request, CancellationToken cancellationToken)
    {
        var objetivos = await _objetivoRepository.GetAllAsync();

        return objetivos.Select(o => new ObjetivoDto
        {
            ObjetivoId = o.ObjetivoId,
            ProyectoId = o.ProyectoId,
            Nombre = o.Nombre,
            Descripcion = o.Descripcion,
            ResultadoEsperado = o.ResultadoEsperado,
            ProyectoNombre = o.Proyecto?.Nombre
        });
    }
}

public class GetObjetivosByProyectoIdHandler : IRequestHandler<GetObjetivosByProyectoIdQuery, IEnumerable<ObjetivoDto>>
{
    private readonly IObjetivoRepository _objetivoRepository;

    public GetObjetivosByProyectoIdHandler(IObjetivoRepository objetivoRepository)
    {
        _objetivoRepository = objetivoRepository;
    }

    public async Task<IEnumerable<ObjetivoDto>> Handle(GetObjetivosByProyectoIdQuery request, CancellationToken cancellationToken)
    {
        var objetivos = await _objetivoRepository.GetByProyectoIdAsync(request.ProyectoId);

        return objetivos.Select(o => new ObjetivoDto
        {
            ObjetivoId = o.ObjetivoId,
            ProyectoId = o.ProyectoId,
            Nombre = o.Nombre,
            Descripcion = o.Descripcion,
            ResultadoEsperado = o.ResultadoEsperado,
            ProyectoNombre = o.Proyecto?.Nombre
        });
    }
}
