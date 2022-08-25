using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Models;
using CCAS.Application.Common.Persistence;
using CCAS.Application.Common.Persistence.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.Students.Queries;

public class GetStudentsForDataGrid : IRequest<PaginatedList<StudentVM>>
{
    //public int PageNo { get; set; } = 1;
    //public int PageSize { get; set; } = 10;
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? StudentNumber { get; set; }

    public string[]? Sort { get; set; } = null;
}

public class GetStudentsForDataGridHandler : IRequestHandler<GetStudentsForDataGrid, PaginatedList<StudentVM>>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetStudentsForDataGridHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<PaginatedList<StudentVM>> Handle(GetStudentsForDataGrid request, CancellationToken cancellationToken)
    {
        IQueryable<Student> query = context.Students;

        if (request.Id != null)
            query = query.Where(p => p.Id == request.Id);
        if (!string.IsNullOrWhiteSpace(request.Name))
            query = query.StringExpressionFilter(p => p.Name, request.Name);
        if (!string.IsNullOrWhiteSpace(request.StudentNumber))
            query = query.StringExpressionFilter(p => p.StudentNumber, request.StudentNumber);

        var totalRecordCount = await query.CountAsync();

        var sortOptions = new SortBuilder<Student>()
            .CreateSortList(request.Sort, new Common.Models.SortOption<Student>(p => p.Name!));

        query = query.ApplySortOptions(sortOptions);

        //query = query.Skip((request.PageNo - 1) * request.PageSize)
        //             .Take(request.PageSize);

        query = query.Skip(request.Skip)
                     .Take(request.Take);

        var items = await query.AsNoTracking()
            .ProjectTo<StudentVM>(mapper.ConfigurationProvider)
            .ToListAsync();

        var a = new PaginatedList<StudentVM>(items, totalRecordCount);
        return a;
    }
}
