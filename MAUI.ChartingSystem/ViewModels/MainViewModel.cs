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
    public class MainViewModel : INotifyPropertyChanged
    {
        public ObservableCollection<Appointment> Appointments
        {
            get
            {
                return new ObservableCollection<Appointment>(ChartServiceProxy.Current.GetAllAppointments());
            }
        }

        public ObservableCollection<Patient> Patients
        {
            get
            {
                return new ObservableCollection<Patient>(ChartServiceProxy.Current.GetAllPatients());
            }
        }

        public ObservableCollection<Physician> Physicians
        {
            get
            {
                return new ObservableCollection<Physician>(ChartServiceProxy.Current.GetAllPhysicians());
            }
        }

        public MainViewModel()
        {
            SetUpCommands();
        }

        public void Refresh()
        {
            NotifyPropertyChanged("Patients");
            NotifyPropertyChanged("Physicians");
            NotifyPropertyChanged("Appointments");
        }

        public Appointment? SelectedAppointment { get; set; }

        public Patient? SelectedPatient { get; set; }

        public Physician? SelectedPhysician { get; set; }

        public ICommand? DeleteAppointmentCommand { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void DeletePatient()
        {
            if (SelectedPatient == null)
            {
                return;
            }
            ChartServiceProxy.Current.RemovePatient(SelectedPatient);
            NotifyPropertyChanged(nameof(Patients));
            NotifyPropertyChanged(nameof(Appointments));
        }

        public void DeletePhysician()
        {
            if (SelectedPhysician == null)
            {
                return;
            }
            ChartServiceProxy.Current.RemovePhysician(SelectedPhysician);
            NotifyPropertyChanged(nameof(Physicians));
            NotifyPropertyChanged(nameof(Appointments));
        }

        public async Task DoDelete(Appointment appointment)
        {
            if (appointment == null)
                return;

            bool confirm = await App.Current.MainPage.DisplayAlert("Confirm", $"Delete {appointment.Display}?", "Yes", "No");

            if (confirm)
                ChartServiceProxy.Current.CancelAppointment(appointment);
                NotifyPropertyChanged(nameof(Appointments));
        }

        private void SetUpCommands()
        {
            DeleteAppointmentCommand = new Command<Appointment>(async (appt) => await DoDelete(appt));
        }
    }
}
