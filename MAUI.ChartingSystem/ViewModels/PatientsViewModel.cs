using Library.ChartingSystem.Models;
using Library.ChartingSystem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUI.ChartingSystem.ViewModels
{
    public class PatientsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private Patient? _patient = null;

        public ObservableCollection<Patient> AllPatients { get; set; } = new();

        public ObservableCollection<Patient> FilteredPatients { get; set; } = new();

        private string _patientSearchText;
        public string PatientSearchText
        {
            get => _patientSearchText;
            set
            {
                if (_patientSearchText != value)
                {
                    _patientSearchText = value;
                    NotifyPropertyChanged();
                    FilterPatients();
                }
            }
        }

        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            private set { _isBusy = value; NotifyPropertyChanged(); }
        }

        public PatientsViewModel()
        {
            //LoadPatients();
            SetUpCommands();
        }

        public async Task LoadPatients()
        {
            try
            {
                IsBusy = true;

                AllPatients.Clear();

                var data = await PatientServiceProxy.Current.GetAll() ?? new List<Patient>();
                foreach (var patient in data)
                    AllPatients.Add(patient);

                FilterPatients();
            }
            catch (Exception ex)
            {
                await App.Current.MainPage.DisplayAlert("Error", $"Failed to load patients. Please try again later.", "OK");
                await Shell.Current.GoToAsync("//MainPage");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void Refresh()
        {
            await LoadPatients();
        }

        public ICommand? DeletePatientCommand {  get; set; }
        public ICommand? EditPatientCommand { get; set; }

        public ICommand? FilterPatientCommand { get; set; }

        private void SetUpCommands()
        {
            DeletePatientCommand = new Command<Patient>(async (patient) => await DeletePatient(patient));
            EditPatientCommand = new Command<Patient>(async (patient) => await EditPatient(patient));
        }

        private void FilterPatients()
        {
            string query = PatientSearchText?.ToLower() ?? "";

            var results = AllPatients
                .Where(p => string.IsNullOrEmpty(query)
                || (p.Name?.ToLower().Contains(query) ?? false))
                .ToList();

            FilteredPatients.Clear();

            foreach (var p in results)
                FilteredPatients.Add(p);
        }

        private async Task DeletePatient(Patient patient)
        {
            if (patient == null)
                return;

            bool confirm = await App.Current.MainPage.DisplayAlert("Confirm", $"Delete {patient.Display}?", "Yes", "No");

            if (confirm)
            {
                await PatientServiceProxy.Current.Delete(patient);
                Refresh();
            }
        }

        private async Task EditPatient(Patient patient)
        {
            if (patient == null)
                return;

            await Shell.Current.GoToAsync($"//Patient?patientId={patient.Id}");
        }



        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
