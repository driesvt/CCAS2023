using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.LSuJoins.Queries;

public class GetLSuJoinById : IRequest<LSuJoinVM>
{
    public int Id { get; set; }
}

public class GetLSuJoinByIdQueryHandler : IRequestHandler<GetLSuJoinById, LSuJoinVM>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetLSuJoinByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<LSuJoinVM> Handle(GetLSuJoinById request, CancellationToken cancellationToken)
    {
        var sSJoin = await context.LSuJoins.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id);
        if (sSJoin == null)
            throw new NotFoundException(nameof(LSuJoin), request.Id);

        return mapper.Map<LSuJoinVM>(sSJoin);
    }
}
