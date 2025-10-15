using Library.ChartingSystem.Models;
using Library.ChartingSystem.Services;
using MAUI.ChartingSystem.ViewModels;

namespace MAUI.ChartingSystem.Views;

public partial class PatientView : ContentPage
{
    private readonly AddPatientViewModel _viewModel = new();

    public PatientView()
    {
        InitializeComponent();
        BindingContext = _viewModel;
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void OkClicked(object sender, EventArgs e)
    {
        var patient = _viewModel.ToPatient();
        ChartServiceProxy.Current.AddPatient(patient);
        Shell.Current.GoToAsync("//MainPage");
    }
}