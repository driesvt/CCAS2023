using CCAS.Application.Lecturers.Commands;
using CCAS.Application.Lecturers.Queries;
using MediatR;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace CCAS.BlazorServer.Pages.SharedCustomAdaptors;

public class LecturersGridCustomAdaptor : BaseCustomAdapter
{

    public LecturersGridCustomAdaptor(IMediator mediator) : base(mediator)
    {
    }


    // Performs data Read operation
    public override async Task<object> ReadAsync(DataManagerRequest dm, string? key = null)
    {

        var dataSrc = await Mediator.Send(
            new GetAllLecturersQuery()
            );

        IEnumerable<LecturerVM> DataSource = dataSrc;

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
        var count = DataSource.Cast<LecturerVM>().Count();
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
        var data1 = data as LecturerVM;

        var insertId = await Mediator.Send(new CreateLecturerCommand()
        {
            Name = data1!.Name,
            LecturerNumber = data1.LecturerNumber,
            ContactNumber = data1.ContactNumber,
            Email = data1.Email,
            PhysicalAddress = data1.PhysicalAddress,
            PostalAddress = data1.PostalAddress,
            InceptionDate = data1.InceptionDate
        });

        var insertedLecturer = await Mediator.Send(new GetLecturerById() { Id = insertId });

        return insertedLecturer;
    }

    public async override Task<object> UpdateAsync(DataManager dataManager, object data, string keyField, string key)
    {
        var data1 = data as LecturerVM;

        await Mediator.Send(new UpdateLecturerCommand()
        {
            Id = data1!.Id,
            Name = data1.Name,
            LecturerNumber = data1.LecturerNumber,
            ContactNumber = data1.ContactNumber,
            Email = data1.Email,
            PhysicalAddress = data1.PhysicalAddress,
            PostalAddress = data1.PostalAddress,
            InceptionDate = data1.InceptionDate
        });
        return data;
    }

    public async override Task<object> RemoveAsync(DataManager dataManager, object data, string keyField, string key)
    {
        await Mediator.Send(new DeleteLecturerCommand() { Id = (int)data });

        return data;
    }
}
