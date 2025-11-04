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
        public ObservableCollection<Patient> Patients
        {
            get
            {
                return new ObservableCollection<Patient>(ChartServiceProxy.Current.GetAllPatients());
            }
        }

        public PatientsViewModel()
        {
            SetUpCommands();
        }

        public void Refresh()
        {
            NotifyPropertyChanged(nameof(Patients));
        }

        public ICommand? DeletePatientCommand {  get; set; }
        public ICommand? EditPatientCommand { get; set; }

        private void SetUpCommands()
        {
            DeletePatientCommand = new Command<Patient>(async (patient) => await DeletePatient(patient));
            EditPatientCommand = new Command<Patient>(async (patient) => await EditPatient(patient));
        }

        private async Task DeletePatient(Patient patient)
        {
            if (patient == null)
                return;

            bool confirm = await App.Current.MainPage.DisplayAlert("Confirm", $"Delete {patient.Display}?", "Yes", "No");

            if (confirm)
            {
                ChartServiceProxy.Current.RemovePatient(patient);
                NotifyPropertyChanged(nameof(Patients));
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
