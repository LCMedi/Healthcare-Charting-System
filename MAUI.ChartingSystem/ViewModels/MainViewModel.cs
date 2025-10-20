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
            SelectedAppointment = null;
            SelectedPhysician = null;
            SelectedPatient = null;
            NotifyPropertyChanged(nameof(Patients));
            NotifyPropertyChanged(nameof(Physicians));
            NotifyPropertyChanged(nameof(Appointments));
        }

        public Appointment? SelectedAppointment { get; set; }

        public Patient? SelectedPatient { get; set; }

        public Physician? SelectedPhysician { get; set; }

        public ICommand? DeleteAppointmentCommand { get; set; }
        public ICommand? EditAppointmentCommand { get; set; }

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
            Refresh();
        }

        public void DeletePhysician()
        {
            if (SelectedPhysician == null)
            {
                return;
            }
            ChartServiceProxy.Current.RemovePhysician(SelectedPhysician);
            Refresh();
        }

        private async Task DeleteAppointment(Appointment appointment)
        {
            if (appointment == null)
                return;

            bool confirm = await App.Current.MainPage.DisplayAlert("Confirm", $"Delete {appointment.Display}?", "Yes", "No");

            if (confirm)
                ChartServiceProxy.Current.CancelAppointment(appointment);
                NotifyPropertyChanged(nameof(Appointments));
        }

        private async Task EditAppointment(Appointment appointment)
        {
            if (appointment == null)
                return;

            await Shell.Current.GoToAsync($"//Appointment?appointmentId={appointment.Id}");
        }

        private void SetUpCommands()
        {
            DeleteAppointmentCommand = new Command<Appointment>(async (appt) => await DeleteAppointment(appt));
            EditAppointmentCommand = new Command<Appointment>(async (appt) => await EditAppointment(appt));
        }
    }
}
