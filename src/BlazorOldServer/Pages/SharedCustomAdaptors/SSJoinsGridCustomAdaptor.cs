using CCAS.Application.SSJoins.Commands;
using CCAS.Application.SSJoins.Queries;
using MediatR;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace CCAS.BlazorOldServer.Pages.SharedCustomAdaptors;

public class SSJoinsGridCustomAdaptor : BaseCustomAdapter
{

    public SSJoinsGridCustomAdaptor(IMediator mediator) : base(mediator)
    {
    }


    // Performs data Read operation
    public override async Task<object> ReadAsync(DataManagerRequest dm, string? key = null)
    {

        var dataSrc = await Mediator.Send(
            new GetAllSSJoinsQuery()
            );

        IEnumerable<SSJoinVM> DataSource = dataSrc;

        //return dm.RequiresCounts ? new DataResult() { Result = dataSource.Items, Count = dataSource.TotalCount } : (object)dataSource.Items;
        if (dm.Search != null && dm.Search.Count > 0)
        {
            // Searching
            DataSource = DataOperations.PerformSearching(DataSource, dm.Search);
        }
        if (dm.Sorted != null && dm.Sorted.Count > 0)
        {
            // Sorting
            DataSource = DataOperations.PerformSorting(DataSource, dm.Sorted);
        }
        if (dm.Where != null && dm.Where.Count > 0)
        {
            // Filtering
            DataSource = DataOperations.PerformFiltering(DataSource, dm.Where, dm.Where[0].Operator);
        }
        var count = DataSource.Cast<SSJoinVM>().Count();
        if (dm.Skip != 0)
        {
            //Paging
            DataSource = DataOperations.PerformSkip(DataSource, dm.Skip);
        }
        if (dm.Take != 0)
        {
            DataSource = DataOperations.PerformTake(DataSource, dm.Take);
        }
        var DataObject = new DataResult();
        if (dm.Aggregates != null) // Aggregation
        {
            DataObject.Result = DataSource;
            DataObject.Count = count;
            DataObject.Aggregates = DataUtil.PerformAggregation(DataSource, dm.Aggregates);

            return dm.RequiresCounts ? DataObject : DataSource;
        }
        return dm.RequiresCounts ? new DataResult() { Result = DataSource, Count = count } : DataSource;

    }

    public override async Task<object> InsertAsync(DataManager dataManager, object data, string key)
    {
        var data1 = data as SSJoinVM;

        var insertId = await Mediator.Send(new CreateSSJoinCommand()
        {
            StudentId = data1!.StudentId,
            SubjectId = data1.SubjectId,
        });

        var insertedSSJoin = await Mediator.Send(new GetSSJoinById() { Id = insertId });

        return insertedSSJoin;
    }

    public async override Task<object> UpdateAsync(DataManager dataManager, object data, string keyField, string key)
    {
        var data1 = data as SSJoinVM;

        await Mediator.Send(new UpdateSSJoinCommand()
        {
            Id = data1!.Id,
            StudentId = data1.StudentId,
            SubjectId = data1.SubjectId,
        });
        return data;
    }

    public async override Task<object> RemoveAsync(DataManager dataManager, object data, string keyField, string key)
    {
        await Mediator.Send(new DeleteSSJoinCommand() { Id = (int)data });

        return data;
    }
}
