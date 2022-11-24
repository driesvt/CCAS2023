using CCAS.Application.Common.Exceptions;
using CCAS.Application.SSJoins.Queries;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Popups;
using BlazorServer.Shared;

namespace BlazorServer.Pages.SSJoins;

public partial class SSJoinsGrid : Microsoft.AspNetCore.Components.ComponentBase
{
    private CustomValidation? _customValidation;
    public SfGrid<SSJoinVM>? Grid { get; set; }
    public SSJoinVM? SelectedData;
    private List<Object> Toolbaritems = new List<Object>() { "Add", "Edit", "Delete", "Cancel", "Update", "ExcelExport", "PdfExport", "CsvExport" };
    private List<Object> Contextmenuitems = new List<Object>() { "AutoFit", "AutoFitAll", "SortAscending", "SortDescending", "Copy", "Edit", "Delete", "Save", "Cancel", "PdfExport", "ExcelExport", "CsvExport", "FirstPage", "PrevPage", "LastPage", "NextPage" };

    [Inject]
    private SfDialogService? DialogService { get; set; }

    public async Task ActionFailureHandler(FailureEventArgs args)
    {
        switch(args.Error)
        {
            case ValidationException ex:

                if (ex.Errors != null)
                {
                    if (_customValidation != null)
                        _customValidation?.DisplayErrors(ex.Errors);
                    else
                    {

                    }
                }
                
                break;
            case DeleteForbiddenException ex:

                await DialogService!.AlertAsync("Error Deleting Record", $"Error Deleting Record: {ex.Message}");
                break;
            default :

                //notificationService.Notify(NotificationSeverity.Error, summary: "Error", detail: $"Error: {args.Error.Message}", duration: 3000);
                break;
        }
    }


    public void RowSelectHandler(RowSelectEventArgs<SSJoinVM> Args)
    {
        SelectedData = Args.Data;
    }


    public async Task OnActionBegin(ActionEventArgs<SSJoinVM> Args)
    {
        if (Args.RequestType.ToString() == "Delete")
        {
            bool? result = await DialogService.ConfirmAsync(
                 "Are you sure you want to delete this record?", "Warning");

            if (result == true)
            {
                Args.Cancel = false;
            }
            else
            {
                Args.Cancel = true;
            }
        }
    }


    public async Task ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
    {
        if (args.Item.Id == "SSJoinsGrid_pdfexport")  //Id is combination of Grid's ID and itemname
        {
            await Grid!.PdfExport();
        }
        if (args.Item.Id == "SSJoinsGrid_excelexport") //Id is combination of Grid's ID and itemname
        {
            await Grid!.ExcelExport();
        }
        if (args.Item.Id == "SSJoinsGrid_csvexport") //Id is combination of Grid's ID and itemname
        {
            await Grid!.CsvExport();
        }

        if (args.Item.Id == "CustomDelete" && SelectedData != null)
        {
            
        }
    }
}

