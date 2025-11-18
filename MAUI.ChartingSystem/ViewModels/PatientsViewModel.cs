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

        public PatientsViewModel()
        {
            LoadPatients();
            SetUpCommands();
        }

        public void LoadPatients()
        {
            AllPatients.Clear();
            var data = ChartServiceProxy.Current.GetAllPatients();

            foreach (var p in data)
                AllPatients.Add(p);

            FilterPatients();
        }

        public void Refresh()
        {
            LoadPatients();
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
                || p.Name.ToLower().Contains(query))
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
                ChartServiceProxy.Current.RemovePatient(patient);
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
