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
    public class AppointmentsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        public ObservableCollection<Appointment> Appointments
        {
            get
            {
                return new ObservableCollection<Appointment>(ChartServiceProxy.Current.GetAllAppointments());
            }
        }

        public AppointmentsViewModel()
        {
            SetUpCommands();
        }

        public ICommand? DeleteAppointmentCommand { get; set; }
        public ICommand? EditAppointmentCommand { get; set; }

        public void Refresh()
        {
            NotifyPropertyChanged(nameof(Appointments));
        }

        private void SetUpCommands()
        {
            DeleteAppointmentCommand = new Command<Appointment>(async (appt) => await DeleteAppointment(appt));
            EditAppointmentCommand = new Command<Appointment>(async (appt) => await EditAppointment(appt));
        }

        private async Task DeleteAppointment(Appointment appointment)
        {
            if (appointment == null)
                return;

            bool confirm = await App.Current.MainPage.DisplayAlert("Confirm", $"Delete {appointment.Display}?", "Yes", "No");

            if (confirm)
            {
                ChartServiceProxy.Current.CancelAppointment(appointment);
                NotifyPropertyChanged(nameof(Appointments));
            }
        }

        private async Task EditAppointment(Appointment appointment)
        {
            if (appointment == null)
                return;

            await Shell.Current.GoToAsync($"//Appointment?appointmentId={appointment.Id}");
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
