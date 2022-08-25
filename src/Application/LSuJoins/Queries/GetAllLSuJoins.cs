using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.LSuJoins.Queries;

public class GetAllLSuJoinsQuery : IRequest<List<LSuJoinVM>>
{

}

public class GetAllLSuJoinsQueryHandler : IRequestHandler<GetAllLSuJoinsQuery, List<LSuJoinVM>>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetAllLSuJoinsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<List<LSuJoinVM>> Handle(GetAllLSuJoinsQuery request, CancellationToken cancellationToken)
    {
        return await context.LSuJoins.AsNoTracking().ProjectTo<LSuJoinVM>(mapper.ConfigurationProvider).OrderBy(p => p.LecturerId).ToListAsync();
    }
}
