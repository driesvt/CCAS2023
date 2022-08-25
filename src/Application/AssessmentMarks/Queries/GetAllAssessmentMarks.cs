using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.AssessmentMarks.Queries;

public class GetAllAssessmentMarksQuery : IRequest<List<AssessmentMarkVM>>
{

}

public class GetAllAssessmentMarksQueryHandler : IRequestHandler<GetAllAssessmentMarksQuery, List<AssessmentMarkVM>>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetAllAssessmentMarksQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<List<AssessmentMarkVM>> Handle(GetAllAssessmentMarksQuery request, CancellationToken cancellationToken)
    {
        return await context.AssessmentMarks.AsNoTracking().ProjectTo<AssessmentMarkVM>(mapper.ConfigurationProvider).OrderBy(p => p.AssessmentId).ToListAsync();
    }
}
