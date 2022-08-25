using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.SSJoins.Queries;

public class GetSSJoinById : IRequest<SSJoinVM>
{
    public int Id { get; set; }
}

public class GetSSJoinByIdQueryHandler : IRequestHandler<GetSSJoinById, SSJoinVM>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetSSJoinByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<SSJoinVM> Handle(GetSSJoinById request, CancellationToken cancellationToken)
    {
        var sSJoin = await context.SSJoins.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id);
        if (sSJoin == null)
            throw new NotFoundException(nameof(SSJoin), request.Id);

        return mapper.Map<SSJoinVM>(sSJoin);
    }
}
