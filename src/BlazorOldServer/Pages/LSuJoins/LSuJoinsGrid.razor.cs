using CCAS.Application.Common.Exceptions;
using CCAS.Application.LSuJoins.Queries;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Popups;
using CCAS.BlazorOldServer.Shared;
using MudBlazor;

namespace CCAS.BlazorOldServer.Pages.LSuJoins;

public partial class LSuJoinsGrid : Microsoft.AspNetCore.Components.ComponentBase
{
    private CustomValidation? _customValidation;
    public SfGrid<LSuJoinVM>? Grid { get; set; }
    public LSuJoinVM? SelectedData;
    private List<Object> Toolbaritems = new List<Object>() { "Add", "Edit", "Delete", "Cancel", "Update", "ExcelExport", "PdfExport", "CsvExport" };
    private List<Object> Contextmenuitems = new List<Object>() { "AutoFit", "AutoFitAll", "SortAscending", "SortDescending", "Copy", "Edit", "Delete", "Save", "Cancel", "PdfExport", "ExcelExport", "CsvExport", "FirstPage", "PrevPage", "LastPage", "NextPage" };

    [Inject]
    private IDialogService? DialogService { get; set; }

    [Inject]
    private ISnackbar? snackbar { get; set; }


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
                        snackbar!.Add($"Error: {ex.Message}", Severity.Error);
                    }
                }
                
                break;
            case DeleteForbiddenException ex:

                await DialogService!.ShowMessageBox("Error Deleting Record", $"Error Deleting Record: {ex.Message}");
                break;
            default :

                //notificationService.Notify(NotificationSeverity.Error, summary: "Error", detail: $"Error: {args.Error.Message}", duration: 3000);
                break;
        }
    }


    public void RowSelectHandler(RowSelectEventArgs<LSuJoinVM> Args)
    {
        SelectedData = Args.Data;
    }


    public async Task OnActionBegin(ActionEventArgs<LSuJoinVM> Args)
    {
        if (Args.RequestType.ToString() == "Delete")
        {
            bool? result = await DialogService!.ShowMessageBox(
                "Warning",
                "Are you sure you want to delete this record?",
                yesText: "Delete!", cancelText: "Cancel");

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
        if (args.Item.Id == "LSuJoinsGrid_pdfexport")  //Id is combination of Grid's ID and itemname
        {
            await Grid!.PdfExport();
        }
        if (args.Item.Id == "LSuJoinsGrid_excelexport") //Id is combination of Grid's ID and itemname
        {
            await Grid!.ExcelExport();
        }
        if (args.Item.Id == "LSuJoinsGrid_csvexport") //Id is combination of Grid's ID and itemname
        {
            await Grid!.CsvExport();
        }

        if (args.Item.Id == "CustomDelete" && SelectedData != null)
        {
            
        }
    }
}

