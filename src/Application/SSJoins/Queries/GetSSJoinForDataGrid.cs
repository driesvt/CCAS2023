﻿using AutoMapper;
using AutoMapper.QueryableExtensions;
using CCAS.Application.Common.Entities;
using CCAS.Application.Common.Models;
using CCAS.Application.Common.Persistence;
using CCAS.Application.Common.Persistence.Helpers;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CCAS.Application.SSJoins.Queries;

public class GetSSJoinForDataGrid : IRequest<PaginatedList<SSJoinVM>>
{
    //public int PageNo { get; set; } = 1;
    //public int PageSize { get; set; } = 10;
    public int Skip { get; set; } = 0;
    public int Take { get; set; } = 10;
    public int? Id { get; set; }
    public string? Name { get; set; }
    public string? SSJoinNumber { get; set; }

    public string[]? Sort { get; set; } = null;
}

public class GetSSJoinForDataGridHandler : IRequestHandler<GetSSJoinForDataGrid, PaginatedList<SSJoinVM>>
{
    private readonly IApplicationDbContext context;
    private readonly IMapper mapper;

    public GetSSJoinForDataGridHandler(IApplicationDbContext context, IMapper mapper)
    {
        this.context = context;
        this.mapper = mapper;
    }

    public async Task<PaginatedList<SSJoinVM>> Handle(GetSSJoinForDataGrid request, CancellationToken cancellationToken)
    {
        IQueryable<SSJoin> query = context.SSJoins;

        if (request.Id != null)
            query = query.Where(p => p.Id == request.Id);

        var totalRecordCount = await query.CountAsync();

        var sortOptions = new SortBuilder<SSJoin>()
            .CreateSortList(request.Sort, new Common.Models.SortOption<SSJoin>(p => p.StudentId));

        query = query.ApplySortOptions(sortOptions);

        //query = query.Skip((request.PageNo - 1) * request.PageSize)
        //             .Take(request.PageSize);

        query = query.Skip(request.Skip)
                     .Take(request.Take);

        var items = await query.AsNoTracking()
            .ProjectTo<SSJoinVM>(mapper.ConfigurationProvider)
            .ToListAsync();

        var a = new PaginatedList<SSJoinVM>(items, totalRecordCount);
        return a;
    }
}
