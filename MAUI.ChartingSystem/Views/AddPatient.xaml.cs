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

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        // If updating patient
        if (PatientId > 0)
        {
            _viewModel = new AddPatientViewModel(PatientId);
            BindingContext = _viewModel;
        }
        // If creating patient
        else
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
}