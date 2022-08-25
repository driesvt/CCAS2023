using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Models;
using CCAS.Application.Common.Persistence;
using CCAS.Application.Common.Persistence.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Subjects.Queries;

public class GetSubjectsForDataGrid : IRequest<PaginatedList<SubjectVM>>
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

public class GetSubjectsForDataGridHandler : IRequestHandler<GetSubjectsForDataGrid, PaginatedList<SubjectVM>>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetSubjectsForDataGridHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<PaginatedList<SubjectVM>> Handle(GetSubjectsForDataGrid request, CancellationToken cancellationToken)
    {
        IQueryable<Subject> query = context.Subjects;

        if (request.Id != null)
            query = query.Where(p => p.Id == request.Id);
        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.StringExpressionFilter(p => p.Name, request.Name);
        if (!string.IsNullOrWhiteSpace(request.Code))
            query = query.StringExpressionFilter(p => p.Code, request.Code);

        var totalRecordCount = await query.CountAsync();

        var sortOptions = new SortBuilder<Subject>()
            .CreateSortList(request.Sort, new Common.Models.SortOption<Subject>(p => p.Name!));

        query = query.ApplySortOptions(sortOptions);

        //query = query.Skip((request.PageNo - 1) * request.PageSize)
        //             .Take(request.PageSize);

        query = query.Skip(request.Skip)
                     .Take(request.Take);

        var items = await query.AsNoTracking()
            .ProjectTo<SubjectVM>(mapper.ConfigurationProvider)
            .ToListAsync();

        var a = new PaginatedList<SubjectVM>(items, totalRecordCount);
        return a;
    }
}
