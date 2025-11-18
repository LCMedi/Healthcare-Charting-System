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
        public ObservableCollection<Appointment> AllAppointments { get; set; } = new();

        public ObservableCollection<Appointment> FilteredAppointments { get; set; } = new();

        private string _appointmentSearchText;
        public string AppointmentSearchText
        {
            get => _appointmentSearchText;
            set
            {
                if (_appointmentSearchText != value)
                {
                    _appointmentSearchText = value;
                    NotifyPropertyChanged();
                    FilterAppointments();
                }
            }
        }

        public AppointmentsViewModel()
        {
            LoadAppointments();
            SetUpCommands();
        }

        public ICommand? DeleteAppointmentCommand { get; set; }
        public ICommand? EditAppointmentCommand { get; set; }

        public void LoadAppointments()
        {
            AllAppointments.Clear();
            var data = ChartServiceProxy.Current.GetAllAppointments();

            foreach (var a in data)
            {
                AllAppointments.Add(a);
            }

            FilterAppointments();
        }

        public void Refresh()
        {
            LoadAppointments();
        }

        private void SetUpCommands()
        {
            DeleteAppointmentCommand = new Command<Appointment>(async (appt) => await DeleteAppointment(appt));
            EditAppointmentCommand = new Command<Appointment>(async (appt) => await EditAppointment(appt));
        }

        private void FilterAppointments()
        {
            string query = AppointmentSearchText?.ToLower() ?? string.Empty;

            var results = AllAppointments
                .Where(a =>
                    string.IsNullOrEmpty(query) ||
                    a.Patient?.Name.ToLower().Contains(query) == true ||
                    a.Physician?.Name.ToLower().Contains(query) == true)
                .ToList();

            FilteredAppointments.Clear();

            foreach (var a in results)
            {
                FilteredAppointments.Add(a);
            }
        }

        private async Task DeleteAppointment(Appointment appointment)
        {
            if (appointment == null)
                return;

            bool confirm = await App.Current.MainPage.DisplayAlert("Confirm", $"Delete {appointment.Display}?", "Yes", "No");

            if (confirm)
            {
                ChartServiceProxy.Current.CancelAppointment(appointment);
                Refresh();
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
