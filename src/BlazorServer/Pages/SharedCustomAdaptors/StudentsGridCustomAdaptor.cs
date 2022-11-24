using CCAS.Application.Students.Commands;
using CCAS.Application.Students.Queries;
using MediatR;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace CCAS.BlazorServer.SharedCustomAdaptors;

public class StudentsGridCustomAdaptor : BaseCustomAdapter
{

    public StudentsGridCustomAdaptor(IMediator mediator) : base(mediator)
    {
    }


    // Performs data Read operation
    public override async Task<object> ReadAsync(DataManagerRequest dm, string? key = null)
    {

        var dataSrc = await Mediator.Send(
            new GetAllStudentsQuery()
            );

        IEnumerable<StudentVM> DataSource = dataSrc;

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
        var count = DataSource.Cast<StudentVM>().Count();
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
        var data1 = data as StudentVM;

        var insertId = await Mediator.Send(new CreateStudentCommand()
        {
            Name = data1!.Name,
            StudentNumber = data1.StudentNumber,
            ContactNumber = data1.ContactNumber,
            Email = data1.Email,
            PhysicalAddress = data1.PhysicalAddress,
            PostalAddress = data1.PostalAddress,
            Year = data1.Year,
            Imagesrc = data1.Imagesrc,
            InceptionDate = data1.InceptionDate
        });

        var insertedStudent = await Mediator.Send(new GetStudentById() { Id = insertId });

        return insertedStudent;
    }

    public async override Task<object> UpdateAsync(DataManager dataManager, object data, string keyField, string key)
    {
        var data1 = data as StudentVM;

        await Mediator.Send(new UpdateStudentCommand()
        {
            Id = data1!.Id,
            Name = data1.Name,
            StudentNumber = data1.StudentNumber,
            ContactNumber = data1.ContactNumber,
            Email = data1.Email,
            PhysicalAddress = data1.PhysicalAddress,
            PostalAddress = data1.PostalAddress,
            Year = data1.Year,
            Imagesrc=data1.Imagesrc,
            InceptionDate = data1.InceptionDate
        });
        return data;
    }

    public async override Task<object> RemoveAsync(DataManager dataManager, object data, string keyField, string key)
    {
        await Mediator.Send(new DeleteStudentCommand() { Id = (int)data });

        return data;
    }
}
