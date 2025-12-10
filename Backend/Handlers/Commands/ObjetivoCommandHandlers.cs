using Backend.Commands.Objetivos;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateObjetivoHandler : IRequestHandler<CreateObjetivoCommand, ObjetivoDto>
{
    private readonly IObjetivoRepository _objetivoRepository;

    public CreateObjetivoHandler(IObjetivoRepository objetivoRepository)
    {
        _objetivoRepository = objetivoRepository;
    }

    public async Task<ObjetivoDto> Handle(CreateObjetivoCommand request, CancellationToken cancellationToken)
    {
        var objetivo = new Objetivo
        {
            ProyectoId = request.ProyectoId,
            Nombre = request.Nombre,
            Descripcion = request.Descripcion,
            ResultadoEsperado = request.ResultadoEsperado
        };

        var createdObjetivo = await _objetivoRepository.CreateAsync(objetivo);

        return new ObjetivoDto
        {
            ObjetivoId = createdObjetivo.ObjetivoId,
            ProyectoId = createdObjetivo.ProyectoId,
            Nombre = createdObjetivo.Nombre,
            Descripcion = createdObjetivo.Descripcion,
            ResultadoEsperado = createdObjetivo.ResultadoEsperado
        };
    }
}

public class UpdateObjetivoHandler : IRequestHandler<UpdateObjetivoCommand, ObjetivoDto>
{
    private readonly IObjetivoRepository _objetivoRepository;

    public UpdateObjetivoHandler(IObjetivoRepository objetivoRepository)
    {
        _objetivoRepository = objetivoRepository;
    }

    public async Task<ObjetivoDto> Handle(UpdateObjetivoCommand request, CancellationToken cancellationToken)
    {
        var objetivo = await _objetivoRepository.GetByIdAsync(request.ObjetivoId);
        if (objetivo == null)
            throw new KeyNotFoundException($"Objetivo with ID {request.ObjetivoId} not found");

        objetivo.Nombre = request.Nombre;
        objetivo.Descripcion = request.Descripcion;
        objetivo.ResultadoEsperado = request.ResultadoEsperado;

        var updatedObjetivo = await _objetivoRepository.UpdateAsync(objetivo);

        return new ObjetivoDto
        {
            ObjetivoId = updatedObjetivo.ObjetivoId,
            ProyectoId = updatedObjetivo.ProyectoId,
            Nombre = updatedObjetivo.Nombre,
            Descripcion = updatedObjetivo.Descripcion,
            ResultadoEsperado = updatedObjetivo.ResultadoEsperado
        };
    }
}

public class DeleteObjetivoHandler : IRequestHandler<DeleteObjetivoCommand, bool>
{
    private readonly IObjetivoRepository _objetivoRepository;

    public DeleteObjetivoHandler(IObjetivoRepository objetivoRepository)
    {
        _objetivoRepository = objetivoRepository;
    }

    public async Task<bool> Handle(DeleteObjetivoCommand request, CancellationToken cancellationToken)
    {
        var exists = await _objetivoRepository.ExistsAsync(request.ObjetivoId);
        if (!exists)
            return false;

        await _objetivoRepository.DeleteAsync(request.ObjetivoId);
        return true;
    }
}
