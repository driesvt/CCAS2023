using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Exceptions;
using CCAS.Application.Common.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Companies.Queries;

public class GetSubjectById : IRequest<CompanyVM>
{
    public int Id { get; set; }
}

public class GetCompanyByIdQueryHandler : IRequestHandler<GetSubjectById, CompanyVM>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetCompanyByIdQueryHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<CompanyVM> Handle(GetSubjectById request, CancellationToken cancellationToken)
    {
        var company = await context.Companies.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.Id);
        if (company == null)
            throw new NotFoundException(nameof(Company), request.Id);

        return mapper.Map<CompanyVM>(company);
    }
}
