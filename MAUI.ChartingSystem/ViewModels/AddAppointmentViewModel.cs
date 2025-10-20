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
    public class AddAppointmentViewModel : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler? PropertyChanged;

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

        private Patient? _selectedPatient = null;
        public Patient? Patient
        {
            get => _selectedPatient;
            set { _selectedPatient = value; OnPropertyChanged(); }
        }
        private Physician? _selectedPhysician = null;
        public Physician? Physician
        {
            get => _selectedPhysician;
            set { _selectedPhysician = value; OnPropertyChanged(); }
        }

        private DateTime? _selectedDate = DateTime.Today;
        public DateTime? Date
        {
            get => _selectedDate;
            set
            {
                if (_selectedDate != value)
                {
                    _selectedDate = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SelectedDateTime));
                }
            }
        }

        
        private TimeSpan? _selectedTime = new TimeSpan(8, 0, 0);
        public TimeSpan? Time
        {
            get => _selectedTime;
            set
            {
                if (_selectedTime != value)
                {
                    _selectedTime = value;
                    OnPropertyChanged();
                    OnPropertyChanged(nameof(SelectedDateTime));
                }
            }
        }

        public DateTime SelectedDateTime
        {
            get
            {
                if (Date.HasValue && Time.HasValue)
                {
                    return Date.Value.Date + Time.Value;
                }
                throw new InvalidOperationException("Date and Time must both be set.");
            }
        }

        public ICommand? DeleteAppointmentCommand { get; set; }
        public ICommand? EditAppointmentCommand { get; set; }
        public ICommand? SaveAppointmentCommand { get; set; }

        // Constructor for Creates
        public AddAppointmentViewModel()
        {
            SetUpCommands();
        }

        // Constructor for Updates
        public AddAppointmentViewModel(int id) : this()
        {
            SetUpCommands();
        }

        private void SetUpCommands()
        {
            SaveAppointmentCommand = new Command(DoSave);
        }

        private void DoSave()
        {
            try
            {
                ChartServiceProxy.Current.ScheduleAppointment(new Appointment(Patient, Physician, SelectedDateTime));
                Shell.Current.DisplayAlert("Success", "Appointment added successfully!", "OK");
                Shell.Current.GoToAsync("//MainPage");
            }
            catch (ArgumentException ex)
            {
                Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Error", $"Something went wrong: {ex.Message}", "OK");
            }
        }

        public void Reset()
        {
            Patient = null;
            Physician = null;
            Date = DateTime.Today;
            Time = new TimeSpan(8, 0, 0);
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}
