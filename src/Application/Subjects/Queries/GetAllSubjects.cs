using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Subjects.Queries;

public class GetAllSubjectsQuery : IRequest<List<SubjectVM>>
{

}

public class GetAllSubjectsQueryHandler : IRequestHandler<GetAllSubjectsQuery, List<SubjectVM>>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetAllSubjectsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<List<SubjectVM>> Handle(GetAllSubjectsQuery request, CancellationToken cancellationToken)
    {
        return await context.Subjects.AsNoTracking().ProjectTo<SubjectVM>(mapper.ConfigurationProvider).OrderBy(p => p.Name).ToListAsync();
    }
}
