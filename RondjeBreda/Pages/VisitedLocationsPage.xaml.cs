using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RondjeBreda.ViewModels;

namespace RondjeBreda.Pages;

/// <summary>
/// The class for the visitedLocationPage with the viewmodel for it.
/// </summary>
public partial class VisitedLocationsPage : ContentPage
{
    private VisitedLocationsViewModel visitedLocationsViewModel;
    public VisitedLocationsPage(VisitedLocationsViewModel visitedLocationsViewModel)
    {
        InitializeComponent();
        this.visitedLocationsViewModel = visitedLocationsViewModel;
        BindingContext = visitedLocationsViewModel;
    }

    protected async override void OnAppearing()
    {
        base.OnAppearing();

        if (visitedLocationsViewModel != null)
        {
            await visitedLocationsViewModel.LoadTestData();
            Debug.WriteLine("Generated test data");
        }
    }
}