using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Models;
using CCAS.Application.Common.Persistence;
using CCAS.Application.Common.Persistence.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.AssessmentMarks.Queries;

public class GetAssessmentMarksForDataGrid : IRequest<PaginatedList<AssessmentMarkVM>>
{
    //public int PageNo { get; set; } = 1;
    //public int PageSize { get; set; } = 10;
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? AssessmentMarkNumber { get; set; }

    public string[]? Sort { get; set; } = null;
}

public class GetAssessmentMarksForDataGridHandler : IRequestHandler<GetAssessmentMarksForDataGrid, PaginatedList<AssessmentMarkVM>>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetAssessmentMarksForDataGridHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<PaginatedList<AssessmentMarkVM>> Handle(GetAssessmentMarksForDataGrid request, CancellationToken cancellationToken)
    {
        IQueryable<AssessmentMark> query = context.AssessmentMarks;

        if (request.Id != null)
            query = query.Where(p => p.Id == request.Id);

        var totalRecordCount = await query.CountAsync();

        var sortOptions = new SortBuilder<AssessmentMark>()
            .CreateSortList(request.Sort, new Common.Models.SortOption<AssessmentMark>(p => p.AssessmentId));

        query = query.ApplySortOptions(sortOptions);

        //query = query.Skip((request.PageNo - 1) * request.PageSize)
        //             .Take(request.PageSize);

        query = query.Skip(request.Skip)
                     .Take(request.Take);

        var items = await query.AsNoTracking()
            .ProjectTo<AssessmentMarkVM>(mapper.ConfigurationProvider)
            .ToListAsync();

        var a = new PaginatedList<AssessmentMarkVM>(items, totalRecordCount);
        return a;
    }
}
