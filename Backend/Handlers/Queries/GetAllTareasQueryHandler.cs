using MediatR;
using AutoMapper;
using Backend.Queries.Tareas;
using Backend.Models.DTOs;
using Backend.Infrastructure.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Backend.Handlers.Queries;

public class GetAllTareasQueryHandler : IRequestHandler<GetAllTareasQuery, IEnumerable<TareaDto>>
{
    private readonly ITareaRepository _repo;
    private readonly IMapper _mapper;

    public GetAllTareasQueryHandler(ITareaRepository repo, IMapper mapper)
    {
        _repo = repo;
        _mapper = mapper;
    }

    public async Task<IEnumerable<TareaDto>> Handle(GetAllTareasQuery request, CancellationToken cancellationToken)
    {
    var list = await _repo.GetAllAsync();
        return _mapper.Map<IEnumerable<TareaDto>>(list);
    }
}
