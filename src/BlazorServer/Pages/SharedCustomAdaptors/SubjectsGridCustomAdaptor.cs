using CCAS.Application.Subjects.Commands;
using CCAS.Application.Subjects.Queries;
using MediatR;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace CCAS.BlazorServer.SharedCustomAdaptors;

public class SubjectsGridCustomAdaptor : BaseCustomAdapter
{

    public SubjectsGridCustomAdaptor(IMediator mediator) : base(mediator)
    {
    }


    // Performs data Read operation
    public override async Task<object> ReadAsync(DataManagerRequest dm, string? key = null)
    {

        var dataSrc = await Mediator.Send(
            new GetAllSubjectsQuery()
            );

        IEnumerable<SubjectVM> DataSource = dataSrc;

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
        var count = DataSource.Cast<SubjectVM>().Count();
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
        var data1 = data as SubjectVM;

        var insertId = await Mediator.Send(new CreateSubjectCommand()
        {
            Name = data1!.Name,
            Code = data1.Code,
            Course = data1.Course,
            Credits = data1.Credits,
            MethodofDelivery = data1.MethodofDelivery,
            NQFLevel = data1.NQFLevel,
            Year = data1.Year,
            Semester = data1.Semester,
            Imagesrc = data1.Imagesrc,
        });

        var insertedSubject = await Mediator.Send(new GetSubjectById() { Id = insertId });

        return insertedSubject;
    }

    public async override Task<object> UpdateAsync(DataManager dataManager, object data, string keyField, string key)
    {
        var data1 = data as SubjectVM;

        await Mediator.Send(new UpdateSubjectCommand()
        {
            Id = data1!.Id,
            Name = data1.Name,
            Code = data1.Code,
            Course = data1.Course,
            Credits = data1.Credits,
            MethodofDelivery = data1.MethodofDelivery,
            NQFLevel = data1.NQFLevel,
            Year = data1.Year,
            Semester = data1.Semester,
            Imagesrc=data1.Imagesrc,
        });
        return data;
    }

    public async override Task<object> RemoveAsync(DataManager dataManager, object data, string keyField, string key)
    {
        await Mediator.Send(new DeleteSubjectCommand() { Id = (int)data });

        return data;
    }
}
