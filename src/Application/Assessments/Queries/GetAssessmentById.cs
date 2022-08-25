using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Assessments.Queries;

public class GetAssessmentById : IRequest<AssessmentVM>
{
    public int Id { get; set; }
}

public class GetAssessmentByIdQueryHandler : IRequestHandler<GetAssessmentById, AssessmentVM>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetAssessmentByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<AssessmentVM> Handle(GetAssessmentById request, CancellationToken cancellationToken)
    {
        var assessment = await context.Assessments.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id);
        if (assessment == null)
            throw new NotFoundException(nameof(Assessment), request.Id);

        return mapper.Map<AssessmentVM>(assessment);
    }
}
