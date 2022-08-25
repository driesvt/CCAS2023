using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.SSJoins.Queries;

public class GetAllSSJoinsQuery : IRequest<List<SSJoinVM>>
{

}

public class GetAllSSJoinsQueryHandler : IRequestHandler<GetAllSSJoinsQuery, List<SSJoinVM>>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetAllSSJoinsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<List<SSJoinVM>> Handle(GetAllSSJoinsQuery request, CancellationToken cancellationToken)
    {
        return await context.SSJoins.AsNoTracking().ProjectTo<SSJoinVM>(mapper.ConfigurationProvider).OrderBy(p => p.StudentId).ToListAsync();
    }
}
