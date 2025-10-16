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
        Shell.Current.GoToAsync("//MainPage");
    }

    private void OkClicked(object sender, EventArgs e)
    {
        if (_viewModel is null)
            return;

        // If editing an existing patient, update it in place; otherwise add a new one.
        if (PatientId > 0)
        {
            var existing = ChartServiceProxy.Current.GetPatient(PatientId);
            if (existing != null)
            {
                existing.SetName(_viewModel.Name);
                existing.SetAddress(_viewModel.Address);
                existing.SetBirthdate(_viewModel.Birthdate);
                existing.SetRace(_viewModel.SelectedRace);
                existing.SetGender(_viewModel.SelectedGender);

                existing.MedicalHistory.Clear();
                foreach (var note in _viewModel.MedicalNotes)
                    existing.MedicalHistory.Add(note);
            }
        }
        else
        {
            var patient = _viewModel.ToPatient();
            ChartServiceProxy.Current.AddPatient(patient);
        }

        Shell.Current.GoToAsync("//MainPage");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if (PatientId > 0)
        {
            _viewModel = new AddPatientViewModel(PatientId);
            BindingContext = _viewModel;
        }
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