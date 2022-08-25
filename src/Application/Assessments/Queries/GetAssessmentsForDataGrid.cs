using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Models;
using CCAS.Application.Common.Persistence;
using CCAS.Application.Common.Persistence.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Assessments.Queries;

public class GetAssessmentsForDataGrid : IRequest<PaginatedList<AssessmentVM>>
{
    //public int PageNo { get; set; } = 1;
    //public int PageSize { get; set; } = 10;
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }

    public string[]? Sort { get; set; } = null;
}

public class GetAssessmentsForDataGridHandler : IRequestHandler<GetAssessmentsForDataGrid, PaginatedList<AssessmentVM>>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetAssessmentsForDataGridHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<PaginatedList<AssessmentVM>> Handle(GetAssessmentsForDataGrid request, CancellationToken cancellationToken)
    {
        IQueryable<Assessment> query = context.Assessments;

        if (request.Id != null)
            query = query.Where(p => p.Id == request.Id);
        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.StringExpressionFilter(p => p.Name, request.Name);
        if (!string.IsNullOrWhiteSpace(request.Code))
            query = query.StringExpressionFilter(p => p.AssessmentCode, request.Code);

        var totalRecordCount = await query.CountAsync();

        var sortOptions = new SortBuilder<Assessment>()
            .CreateSortList(request.Sort, new Common.Models.SortOption<Assessment>(p => p.Name!));

        query = query.ApplySortOptions(sortOptions);

        //query = query.Skip((request.PageNo - 1) * request.PageSize)
        //             .Take(request.PageSize);

        query = query.Skip(request.Skip)
                     .Take(request.Take);

        var items = await query.AsNoTracking()
            .ProjectTo<AssessmentVM>(mapper.ConfigurationProvider)
            .ToListAsync();

        var a = new PaginatedList<AssessmentVM>(items, totalRecordCount);
        return a;
    }
}
