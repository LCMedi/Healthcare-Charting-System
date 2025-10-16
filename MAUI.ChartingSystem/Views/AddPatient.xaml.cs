using Library.ChartingSystem.Models;
using Library.ChartingSystem.Services;
using MAUI.ChartingSystem.ViewModels;

namespace MAUI.ChartingSystem.Views;

public partial class PatientView : ContentPage
{
    private AddPatientViewModel? _viewModel;

    public PatientView()
    {
        InitializeComponent();
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void OkClicked(object sender, EventArgs e)
    {
        if (_viewModel is null)
            return;

        var patient = _viewModel.ToPatient();
        ChartServiceProxy.Current.AddPatient(patient);
        Shell.Current.GoToAsync("//MainPage");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if (_viewModel is null)
        {
            _viewModel = new AddPatientViewModel();
            BindingContext = _viewModel;
        }
        else
        {
            _viewModel.Reset();
        }
    }
}