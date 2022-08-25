using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Subjects.Queries;

public class GetSubjectById : IRequest<SubjectVM>
{
    public int Id { get; set; }
}

public class GetSubjectByIdQueryHandler : IRequestHandler<GetSubjectById, SubjectVM>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetSubjectByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<SubjectVM> Handle(GetSubjectById request, CancellationToken cancellationToken)
    {
        var subject = await context.Subjects.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id);
        if (subject == null)
            throw new NotFoundException(nameof(Subject), request.Id);

        return mapper.Map<SubjectVM>(subject);
    }
}
