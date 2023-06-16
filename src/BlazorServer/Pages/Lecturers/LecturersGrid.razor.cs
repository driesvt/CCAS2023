using CCAS.Application.Common.Exceptions;
using CCAS.Application.Lecturers.Queries;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Popups;
using BlazorServer.Shared;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Charts.Chart.Internal;

namespace BlazorServer.Pages.Lecturers;

public partial class LecturersGrid : Microsoft.AspNetCore.Components.ComponentBase
{
    private CustomValidation? _customValidation;
    public SfGrid<LecturerVM>? Grid { get; set; }
    public LecturerVM? SelectedData;
    private List<Object> Toolbaritems = new List<Object>() { "Add", "Edit", "Delete", "Cancel", "Update", "ExcelExport", "PdfExport", "CsvExport" };
    private List<Object> Contextmenuitems = new List<Object>() { "AutoFit", "AutoFitAll", "SortAscending", "SortDescending", "Copy", "Edit", "Delete", "Save", "Cancel", "PdfExport", "ExcelExport", "CsvExport", "FirstPage", "PrevPage", "LastPage", "NextPage" };
    public string UploadedFile { get; set; }
    public string UploadedPath { get; set; } = string.Empty;

    [Inject]
    private SfDialogService? DialogService { get; set; }

    public async Task ActionFailureHandler(Syncfusion.Blazor.Grids.FailureEventArgs args)
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


    public void RowSelectHandler(RowSelectEventArgs<LecturerVM> Args)
    {
        SelectedData = Args.Data;
    }


    public async Task OnActionBegin(ActionEventArgs<LecturerVM> Args)
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

        if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save && Args.Action == "Add")
        {
            //Args.Data.OrderID = DefaultValue++;    //set the default value while adding.
            //save the file name / url in grid datasource.
            Args.Data.Imagesrc = "Images/Uploads/" + UploadedFile;
            UploadedPath = Args.Data.Imagesrc;
        }
        else if (Args.RequestType == Syncfusion.Blazor.Grids.Action.Save && Args.Action == "Edit")
        {
            if (UploadedFile?.Length > 0)
            {
                Args.Data.Imagesrc = "Images/Uploads/" + UploadedFile;
            }
            UploadedPath = Args.Data.Imagesrc ?? string.Empty;
        }

        Args.PreventRender = false;
    }

    public void OnActionComplete(ActionEventArgs<LecturerVM> args)
    {
        if (args.RequestType.Equals(Syncfusion.Blazor.Grids.Action.Add) || args.RequestType.Equals(Syncfusion.Blazor.Grids.Action.BeginEdit))
        {
            args.PreventRender = false;
        }
        UploadedPath = string.Empty;
    }

    private void OnChange(UploadChangeEventArgs args)
    {
        foreach (var file in args.Files)
        {
            var path = @"./wwwroot/Images/Uploads/" + file.FileInfo.Name;
            FileStream filestream = new FileStream(path, FileMode.Create, FileAccess.Write);
            file.Stream.WriteTo(filestream);
            filestream.Close();
            file.Stream.Close();
        }
    }

    public void Selected(SelectedEventArgs Args)
    {
        UploadedFile = Args.FilesData[0].Name;
        UploadedPath = @"Images/Uploads/" + UploadedFile;
    }
    public async Task ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
    {
        if (args.Item.Id == "LecturersGrid_pdfexport")  //Id is combination of Grid's ID and itemname
        {
            await Grid!.PdfExport();
        }
        if (args.Item.Id == "LecturersGrid_excelexport") //Id is combination of Grid's ID and itemname
        {
            await Grid!.ExcelExport();
        }
        if (args.Item.Id == "LecturersGrid_csvexport") //Id is combination of Grid's ID and itemname
        {
            await Grid!.CsvExport();
        }

        if (args.Item.Id == "CustomDelete" && SelectedData != null)
        {
            
        }
    }
}

