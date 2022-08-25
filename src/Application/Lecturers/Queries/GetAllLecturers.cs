using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Lecturers.Queries;

public class GetAllLecturersQuery : IRequest<List<LecturerVM>>
{

}

public class GetAllLecturersQueryHandler : IRequestHandler<GetAllLecturersQuery, List<LecturerVM>>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetAllLecturersQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<List<LecturerVM>> Handle(GetAllLecturersQuery request, CancellationToken cancellationToken)
    {
        return await context.Lecturers.AsNoTracking().ProjectTo<LecturerVM>(mapper.ConfigurationProvider).OrderBy(p => p.Name).ToListAsync();
    }
}
