using CCAS.Application.Companies.Commands;
using CCAS.Application.Companies.Queries;
using MediatR;
using Syncfusion.Blazor;
using Syncfusion.Blazor.Data;

namespace CCAS.BlazorServer.Pages.SharedCustomAdaptors;

public class CompaniesGridCustomAdaptor : BaseCustomAdapter
{

    public CompaniesGridCustomAdaptor(IMediator mediator) : base(mediator)
    {
    }


    // Performs data Read operation
    public override async Task<object> ReadAsync(DataManagerRequest dm, string? key = null)
    {
        var dataSource = await Mediator.Send(
            new GetCompaniesForDataGrid()
            {
                Skip = dm.Skip,
                Take = dm.Take,
                Name = GetFieldFilter(dm, nameof(CompanyVM.Name)) ?? "",
                Website = GetFieldFilter(dm, nameof(CompanyVM.Website)) ?? "",
                Email = GetFieldFilter(dm, nameof(CompanyVM.Email)) ?? "",
                Sort = GetSort(dm, nameof(CompanyVM.Name)).ToArray()
            });

        return dm.RequiresCounts ? new DataResult() { Result = dataSource.Items, Count = dataSource.TotalCount } : dataSource.Items;
    }

    public override async Task<object> InsertAsync(DataManager dataManager, object data, string key)
    {
        var data1 = data as CompanyVM;

        var insertId = await Mediator.Send(new CreateCompanyCommand()
        {
            Name = data1!.Name,
            ContactNumber = data1.ContactNumber,
            Email = data1.Email,
            PhysicalAddress = data1.PhysicalAddress,
            PostalAddress = data1.PostalAddress,
            Website = data1.Website
        });

        var insertedCompany = await Mediator.Send(new GetSubjectById() { Id = insertId });

        return insertedCompany;
    }

    public async override Task<object> UpdateAsync(DataManager dataManager, object data, string keyField, string key)
    {
        var data1 = data as CompanyVM;

        await Mediator.Send(new UpdateCompanyCommand()
        {
            Id = data1!.Id,
            Name = data1.Name,
            ContactNumber = data1.ContactNumber,
            Email = data1.Email,
            PhysicalAddress = data1.PhysicalAddress,
            PostalAddress = data1.PostalAddress,
            Website = data1.Website
        });
        return data;
    }

    public async override Task<object> RemoveAsync(DataManager dataManager, object data, string keyField, string key)
    {
        await Mediator.Send(new DeleteCompanyCommand() { Id = (int)data });

        return data;
    }
}
