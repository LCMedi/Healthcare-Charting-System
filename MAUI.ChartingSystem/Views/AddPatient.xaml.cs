using Library.ChartingSystem.Models;
using Library.ChartingSystem.Services;
using MAUI.ChartingSystem.ViewModels;

namespace MAUI.ChartingSystem.Views;

[QueryProperty(nameof(PatientId), "patientId")]

public partial class PatientView : ContentPage
{
    private AddPatientViewModel? _viewModel;

    public int PatientId { get; set; }

    public PatientView()
    {
        InitializeComponent();
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Patients");
    }


    private async void OkClicked(object sender, EventArgs e)
    {
        if (_viewModel is null)
            return;

        // If updating patient
        if (PatientId > 0)
        {
            var existing = ChartServiceProxy.Current.GetPatient(PatientId);
            if (existing is null)
            {
                await DisplayAlert("Error", $"Patient with ID {PatientId} was not found.", "OK");
                return;
            }

            if (!_viewModel.TryUpdateExistingPatient(existing, out var error))
            {
                await DisplayAlert("Invalid data", error, "OK");
                return;
            }
        }
        // If creating patient
        else
        {
            if (!_viewModel.TryCreatePatient(out var patient, out var error))
            {
                await DisplayAlert("Invalid data", error, "OK");
                return;
            }

            ChartServiceProxy.Current.AddPatient(patient!);
        }

        await Shell.Current.GoToAsync("//Patients");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        // If updating patient
        if (PatientId > 0)
        {
            _viewModel = new AddPatientViewModel(PatientId);
            _viewModel.Refresh();
            BindingContext = _viewModel;
        }
        // If creating patient
        else
        {
            if (_viewModel is null)
            {
                _viewModel = new AddPatientViewModel();
                _viewModel.Refresh();
                BindingContext = _viewModel;
            }
            else
            {
                _viewModel.Reset();
                _viewModel.Refresh();
            }
        }
    }
}