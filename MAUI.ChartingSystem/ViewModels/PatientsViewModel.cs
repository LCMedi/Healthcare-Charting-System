using Library.ChartingSystem.Models;
using Library.ChartingSystem.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;

namespace MAUI.ChartingSystem.ViewModels
{
    public class PatientsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private Patient? _patient = null;

        public ObservableCollection<Patient> AllPatients { get; } = new();
        public ObservableCollection<Patient> FilteredPatients { get; } = new();

        private string _searchQuery;
        public string SearchQuery
        {
            get => _searchQuery;
            set
            {
                if (_searchQuery == value)
                    return;
                _searchQuery = value;
                NotifyPropertyChanged();
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

                ReplaceFiltered(AllPatients);
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Failed to load patients. Please try again later.", "OK");
                await Shell.Current.GoToAsync("//MainPage");
            }
            finally
            {
                IsBusy = false;
            }
        }

        public async void Refresh()
        {
            if (string.IsNullOrWhiteSpace(SearchQuery))
                await LoadPatients();
            else
                await ExecuteSearchAsync();
        }

        public ICommand? DeletePatientCommand { get; private set; }
        public ICommand? EditPatientCommand { get; private set; }
        public ICommand? SearchCommand { get; private set; }

        private void SetUpCommands()
        {
            DeletePatientCommand = new Command<Patient>(async p => await DeletePatient(p));
            EditPatientCommand = new Command<Patient>(async p => await EditPatient(p));
            SearchCommand = new Command(async () => await ExecuteSearchAsync());
        }

        public async Task ExecuteSearchAsync()
        {
            if (IsBusy) return;

            if (string.IsNullOrWhiteSpace(SearchQuery))
            {
                ReplaceFiltered(AllPatients);
                return;
            }

            await SearchPatientsAsync(SearchQuery);
        }

        private async Task SearchPatientsAsync(string query)
        {
            try
            {
                IsBusy = true;

                var results = await PatientServiceProxy.Current.Search(query) ?? new List<Patient>();
                ReplaceFiltered(results);
            }
            catch (Exception)
            {
                await App.Current.MainPage.DisplayAlert("Error", "Failed to search patients. Please try again later.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void ReplaceFiltered(IEnumerable<Patient> patients)
        {
            FilteredPatients.Clear();
            foreach (var p in patients)
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
