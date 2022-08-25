using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Assessments.Queries;

public class GetAllAssessmentsQuery : IRequest<List<AssessmentVM>>
{

}

public class GetAllAssessmentsQueryHandler : IRequestHandler<GetAllAssessmentsQuery, List<AssessmentVM>>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetAllAssessmentsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<List<AssessmentVM>> Handle(GetAllAssessmentsQuery request, CancellationToken cancellationToken)
    {
        return await context.Assessments.AsNoTracking().ProjectTo<AssessmentVM>(mapper.ConfigurationProvider).OrderBy(p => p.Name).ToListAsync();
    }
}
