using MediatR;
using AutoMapper;
using Backend.Queries.Tareas;
using Backend.Models.DTOs;
using Backend.Infrastructure.Repositories;

namespace Backend.Handlers.Queries;

public class GetTareaByIdQueryHandler : IRequestHandler<GetTareaByIdQuery, TareaDto>
{
    private readonly ITareaRepository _repo;
    private readonly IMapper _mapper;

    public GetTareaByIdQueryHandler(ITareaRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<TareaDto> Handle(GetTareaByIdQuery request, CancellationToken cancellationToken)
    {
    var e = await _repo.GetByIdAsync(request.TareaId);
        return _mapper.Map<TareaDto>(e);
    }
}
