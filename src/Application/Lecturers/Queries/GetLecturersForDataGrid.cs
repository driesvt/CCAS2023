using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Models;
using CCAS.Application.Common.Persistence;
using CCAS.Application.Common.Persistence.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Lecturers.Queries;

public class GetLecturersForDataGrid : IRequest<PaginatedList<LecturerVM>>
{
    //public int PageNo { get; set; } = 1;
    //public int PageSize { get; set; } = 10;
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? LecturerNumber { get; set; }

    public string[]? Sort { get; set; } = null;
}

public class GetLecturersForDataGridHandler : IRequestHandler<GetLecturersForDataGrid, PaginatedList<LecturerVM>>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetLecturersForDataGridHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<PaginatedList<LecturerVM>> Handle(GetLecturersForDataGrid request, CancellationToken cancellationToken)
    {
        IQueryable<Lecturer> query = context.Lecturers;

        if (request.Id != null)
            query = query.Where(p => p.Id == request.Id);
        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.StringExpressionFilter(p => p.Name, request.Name);
        if (!string.IsNullOrWhiteSpace(request.LecturerNumber))
            query = query.StringExpressionFilter(p => p.LecturerNumber, request.LecturerNumber);

        var totalRecordCount = await query.CountAsync();

        var sortOptions = new SortBuilder<Lecturer>()
            .CreateSortList(request.Sort, new Common.Models.SortOption<Lecturer>(p => p.Name!));

        query = query.ApplySortOptions(sortOptions);

        //query = query.Skip((request.PageNo - 1) * request.PageSize)
        //             .Take(request.PageSize);

        query = query.Skip(request.Skip)
                     .Take(request.Take);

        var items = await query.AsNoTracking()
            .ProjectTo<LecturerVM>(mapper.ConfigurationProvider)
            .ToListAsync();

        var a = new PaginatedList<LecturerVM>(items, totalRecordCount);
        return a;
    }
}
