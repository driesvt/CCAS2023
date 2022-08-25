using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.AssessmentMarks.Queries;

public class GetAssessmentMarkById : IRequest<AssessmentMarkVM>
{
    public int Id { get; set; }
}

public class GetAssessmentMarkByIdQueryHandler : IRequestHandler<GetAssessmentMarkById, AssessmentMarkVM>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetAssessmentMarkByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<AssessmentMarkVM> Handle(GetAssessmentMarkById request, CancellationToken cancellationToken)
    {
        var assessmentMark = await context.AssessmentMarks.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id);
        if (assessmentMark == null)
            throw new NotFoundException(nameof(AssessmentMark), request.Id);

        return mapper.Map<AssessmentMarkVM>(assessmentMark);
    }
}
