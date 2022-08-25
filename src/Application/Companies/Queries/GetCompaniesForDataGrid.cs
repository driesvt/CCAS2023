using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Models;
using CCAS.Application.Common.Persistence;
using CCAS.Application.Common.Persistence.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Companies.Queries;

public class GetCompaniesForDataGrid : IRequest<PaginatedList<CompanyVM>>
{
    //public int PageNo { get; set; } = 1;
    //public int PageSize { get; set; } = 10;
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Website { get; set; }

    public string[]? Sort { get; set; } = null;
}

public class GetCompaniesForDataGridHandler : IRequestHandler<GetCompaniesForDataGrid, PaginatedList<CompanyVM>>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetCompaniesForDataGridHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<PaginatedList<CompanyVM>> Handle(GetCompaniesForDataGrid request, CancellationToken cancellationToken)
    {
        IQueryable<Company> query = context.Companies;

        if (request.Id != null)
            query = query.Where(p => p.Id == request.Id);
        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.StringExpressionFilter(p => p.Name, request.Name);
        if (!string.IsNullOrWhiteSpace(request.Email))
            query = query.StringExpressionFilter(p => p.Email, request.Email);
        if (!string.IsNullOrWhiteSpace(request.Website))
            query = query.StringExpressionFilter(p => p.Website, request.Website);

        var totalRecordCount = await query.CountAsync();

        var sortOptions = new SortBuilder<Company>()
            .CreateSortList(request.Sort, new Common.Models.SortOption<Company>(p => p.Name!));

        query = query.ApplySortOptions(sortOptions);

        //query = query.Skip((request.PageNo - 1) * request.PageSize)
        //             .Take(request.PageSize);

        query = query.Skip(request.Skip)
                     .Take(request.Take);

        var items = await query.AsNoTracking()
            .ProjectTo<CompanyVM>(mapper.ConfigurationProvider)
            .ToListAsync();

        var a = new PaginatedList<CompanyVM>(items, totalRecordCount);
        return a;
    }
}
