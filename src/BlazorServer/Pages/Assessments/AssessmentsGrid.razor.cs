using CCAS.Application.Common.Exceptions;
using CCAS.Application.Assessments.Queries;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Popups;
using BlazorServer.Shared;
using CCAS.BlazorServer.Services;

namespace BlazorServer.Pages.Assessments;

public partial class AssessmentsGrid : Microsoft.AspNetCore.Components.ComponentBase
{
    private CustomValidation? _customValidation;
    public SfGrid<AssessmentVM>? Grid { get; set; }
    public AssessmentVM? SelectedData;
    private List<object> Toolbaritems = new() { "Add", "Edit", "Delete", "Cancel", "Update", "ExcelExport", "PdfExport", "CsvExport" };
    private List<object> Contextmenuitems = new() { "AutoFit", "AutoFitAll", "SortAscending", "SortDescending", "Copy", "Edit", "Delete", "Save", "Cancel", "PdfExport", "ExcelExport", "CsvExport", "FirstPage", "PrevPage", "LastPage", "NextPage" };

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


    public void RowSelectHandler(RowSelectEventArgs<AssessmentVM> Args)
    {
        SelectedData = Args.Data;
    }


    public async Task OnActionBegin(ActionEventArgs<AssessmentVM> Args)
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
        if (args.Item.Id == "AssessmentsGrid_pdfexport")  //Id is combination of Grid's ID and itemname
        {
            await Grid!.PdfExport();
        }
        if (args.Item.Id == "AssessmentsGrid_excelexport") //Id is combination of Grid's ID and itemname
        {
            await Grid!.ExcelExport();
        }
        if (args.Item.Id == "AssessmentsGrid_csvexport") //Id is combination of Grid's ID and itemname
        {
            await Grid!.CsvExport();
        }

        if (args.Item.Id == "CustomDelete" && SelectedData != null)
        {
            
        }
    }
}

