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

        }

        public void Refresh()
        {
            NotifyPropertyChanged(nameof(Patients));
        }

        public ICommand? DeletePatientsClicked {  get; set; }
        public ICommand? EditPatientsClicked { get; set; }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }


    }
}
