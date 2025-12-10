using AutoMapper;
using Backend.Commands.Rubros;
using Backend.Infrastructure.Repositories;
using Backend.Models.Domain;
using Backend.Models.DTOs;
using MediatR;

namespace Backend.Handlers.Commands;

public class CreateRubroHandler : IRequestHandler<CreateRubroCommand, RubroDto>
{
    private readonly IRubroRepository _repository;
    private readonly IMapper _mapper;

    public CreateRubroHandler(IRubroRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RubroDto> Handle(CreateRubroCommand request, CancellationToken cancellationToken)
    {
        var rubro = new Rubro
        {
            Descripcion = request.Descripcion
        };

        var created = await _repository.CreateAsync(rubro);
        return _mapper.Map<RubroDto>(created);
    }
}

public class UpdateRubroHandler : IRequestHandler<UpdateRubroCommand, RubroDto>
{
    private readonly IRubroRepository _repository;
    private readonly IMapper _mapper;

    public UpdateRubroHandler(IRubroRepository repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public async Task<RubroDto> Handle(UpdateRubroCommand request, CancellationToken cancellationToken)
    {
        var rubro = await _repository.GetByIdAsync(request.RubroId);
        if (rubro == null)
        {
            throw new KeyNotFoundException($"Rubro con ID {request.RubroId} no encontrado");
        }

        rubro.Descripcion = request.Descripcion;

        var updated = await _repository.UpdateAsync(rubro);
        return _mapper.Map<RubroDto>(updated);
    }
}

public class DeleteRubroHandler : IRequestHandler<DeleteRubroCommand, bool>
{
    private readonly IRubroRepository _repository;

    public DeleteRubroHandler(IRubroRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteRubroCommand request, CancellationToken cancellationToken)
    {
        var exists = await _repository.ExistsAsync(request.RubroId);
        if (!exists)
        {
            throw new KeyNotFoundException($"Rubro con ID {request.RubroId} no encontrado");
        }

        return await _repository.DeleteAsync(request.RubroId);
    }
}
