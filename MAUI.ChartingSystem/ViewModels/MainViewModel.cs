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

        public void Refresh()
        {
            NotifyPropertyChanged("Patients");
        }

        public Appointment? SelectedAppointment { get; set; }

        public Patient? SelectedPatient { get; set; }

        public Physician? SelectedPhysician { get; set; }

        public event PropertyChangedEventHandler? PropertyChanged;

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public void Delete()
        {
            if (SelectedPatient == null)
            {
                return;
            }
            ChartServiceProxy.Current.RemovePatient(SelectedPatient);
            NotifyPropertyChanged(nameof(Patients));
        }
    }
}
