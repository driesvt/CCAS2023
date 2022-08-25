using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Students.Queries;

public class GetAllStudentsQuery : IRequest<List<StudentVM>>
{

}

public class GetAllStudentsQueryHandler : IRequestHandler<GetAllStudentsQuery, List<StudentVM>>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetAllStudentsQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<List<StudentVM>> Handle(GetAllStudentsQuery request, CancellationToken cancellationToken)
    {
        return await context.Students.AsNoTracking().ProjectTo<StudentVM>(mapper.ConfigurationProvider).OrderBy(p => p.Name).ToListAsync();
    }
}
