﻿using CCAS.Application.Common.Exceptions;
using CCAS.Application.Students.Queries;
using CCAS.Application.AssessmentMarks.Queries;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Popups;
using CCAS.BlazorOldServer.Shared;
using MudBlazor;

namespace CCAS.BlazorOldServer.Pages.Students;

public partial class StudentAssessmentMarksGrid : Microsoft.AspNetCore.Components.ComponentBase
{
    private CustomValidation? _customValidation;
    public SfGrid<StudentVM>? Grid { get; set; }
    public SfGrid<AssessmentMarkVM>? Grid2 { get; set; }
    private readonly List<object> _toolbaritems = new() { "Add", "Edit", "Delete", "Cancel", "Update", "ExcelExport", "PdfExport", "CsvExport" };
    private readonly List<object> _contextmenuitems = new() { "AutoFit", "AutoFitAll", "SortAscending", "SortDescending", "Copy", "Edit", "Delete", "Save", "Cancel", "PdfExport", "ExcelExport", "CsvExport", "FirstPage", "PrevPage", "LastPage", "NextPage" };

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
    public StudentVM? SelectedData;
    public string? SelectedStudent { get; set; }
    public int RowIndex { get; set; } = 2;
    public void RowSelectHandler(RowSelectEventArgs<StudentVM> Args)
    {
        SelectedStudent = Args.Data.Name;
        RowIndex = Args.Data.Id;
        SelectedData = Args.Data;
    }


    public async Task OnActionBegin(ActionEventArgs<StudentVM> Args)
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
        if (args.Item.Id == "StudentsGrid_pdfexport")  //Id is combination of Grid's ID and itemname
        {
            await Grid!.PdfExport();
        }
        if (args.Item.Id == "StudentsGrid_excelexport") //Id is combination of Grid's ID and itemname
        {
            await Grid!.ExcelExport();
        }
        if (args.Item.Id == "StudentsGrid_csvexport") //Id is combination of Grid's ID and itemname
        {
            await Grid!.CsvExport();
        }

        if (args.Item.Id == "CustomDelete" && SelectedData != null)
        {
            
        }
    }
}

