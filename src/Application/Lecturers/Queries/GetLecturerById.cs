using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Lecturers.Queries;

public class GetLecturerById : IRequest<LecturerVM>
{
    public int Id { get; set; }
}

public class GetLecturerByIdQueryHandler : IRequestHandler<GetLecturerById, LecturerVM>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetLecturerByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<LecturerVM> Handle(GetLecturerById request, CancellationToken cancellationToken)
    {
        var lecturer = await context.Lecturers.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id);
        if (lecturer == null)
            throw new NotFoundException(nameof(Lecturer), request.Id);

        return mapper.Map<LecturerVM>(lecturer);
    }
}
