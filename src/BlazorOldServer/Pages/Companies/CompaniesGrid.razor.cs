﻿using CCAS.Application.Common.Exceptions;
using CCAS.Application.Companies.Queries;
using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Popups;
using CCAS.BlazorOldServer.Shared;
using MudBlazor;

namespace CCAS.BlazorOldServer.Pages.Companies;

public partial class CompaniesGrid : Microsoft.AspNetCore.Components.ComponentBase
{
    private CustomValidation? _customValidation;
    public SfGrid<CompanyVM>? Grid { get; set; }
    public CompanyVM? SelectedData;
    private List<Object> Toolbaritems = new List<Object>() { "Add", "Edit", "Update", "Delete", "Cancel" };

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


    public void RowSelectHandler(RowSelectEventArgs<CompanyVM> Args)
    {
        SelectedData = Args.Data;
    }


    public async Task OnActionBegin(ActionEventArgs<CompanyVM> Args)
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


    public void ToolbarClickHandler(Syncfusion.Blazor.Navigations.ClickEventArgs args)
    {

        if (args.Item.Id == "CustomDelete" && SelectedData != null)
        {

        }
    }
}

