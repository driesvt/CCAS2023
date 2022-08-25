using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Models;
using CCAS.Application.Common.Persistence;
using CCAS.Application.Common.Persistence.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.LSuJoins.Queries;

public class GetLSuJoinForDataGrid : IRequest<PaginatedList<LSuJoinVM>>
{
    //public int PageNo { get; set; } = 1;
    //public int PageSize { get; set; } = 10;
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? LSuJoinNumber { get; set; }

    public string[]? Sort { get; set; } = null;
}

public class GetLSuJoinForDataGridHandler : IRequestHandler<GetLSuJoinForDataGrid, PaginatedList<LSuJoinVM>>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetLSuJoinForDataGridHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<PaginatedList<LSuJoinVM>> Handle(GetLSuJoinForDataGrid request, CancellationToken cancellationToken)
    {
        IQueryable<LSuJoin> query = context.LSuJoins;

        if (request.Id != null)
            query = query.Where(p => p.Id == request.Id);

        var totalRecordCount = await query.CountAsync();

        var sortOptions = new SortBuilder<LSuJoin>()
            .CreateSortList(request.Sort, new Common.Models.SortOption<LSuJoin>(p => p.LecturerId));

        query = query.ApplySortOptions(sortOptions);

        //query = query.Skip((request.PageNo - 1) * request.PageSize)
        //             .Take(request.PageSize);

        query = query.Skip(request.Skip)
                     .Take(request.Take);

        var items = await query.AsNoTracking()
            .ProjectTo<LSuJoinVM>(mapper.ConfigurationProvider)
            .ToListAsync();

        var a = new PaginatedList<LSuJoinVM>(items, totalRecordCount);
        return a;
    }
}
