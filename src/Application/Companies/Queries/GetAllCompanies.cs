using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Companies.Queries;

public class GetAllCompaniesQuery : IRequest<List<CompanyVM>>
{

}

public class GetAllCompaniesQueryHandler : IRequestHandler<GetAllCompaniesQuery, List<CompanyVM>>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetAllCompaniesQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<List<CompanyVM>> Handle(GetAllCompaniesQuery request, CancellationToken cancellationToken)
    {
        return await context.Companies.AsNoTracking().ProjectTo<CompanyVM>(mapper.ConfigurationProvider).OrderBy(p => p.Name).ToListAsync();
    }
}
