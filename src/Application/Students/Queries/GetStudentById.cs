using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Students.Queries;

public class GetStudentById : IRequest<StudentVM>
{
    public int Id { get; set; }
}

public class GetStudentByIdQueryHandler : IRequestHandler<GetStudentById, StudentVM>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetStudentByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<StudentVM> Handle(GetStudentById request, CancellationToken cancellationToken)
    {
        var student = await context.Students.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id);
        if (student == null)
            throw new NotFoundException(nameof(Student), request.Id);

        return mapper.Map<StudentVM>(student);
    }
}
