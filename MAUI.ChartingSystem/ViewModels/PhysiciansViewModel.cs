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
    public class PhysiciansViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private Physician? _physician = null;

        public ObservableCollection<Physician> Physicians
        {
            get
            {
                return new ObservableCollection<Physician>(ChartServiceProxy.Current.GetAllPhysicians());
            }
        }

        public PhysiciansViewModel()
        {
            SetUpCommands();
        }

        public void Refresh()
        {
            NotifyPropertyChanged(nameof(Physicians));
        }

        public ICommand? DeletePhysicianCommand { get; set; }
        public ICommand? EditPhysicianCommand { get; set; }

        private void SetUpCommands()
        {
            DeletePhysicianCommand = new Command<Physician>(async (physician) => await DeletePhysician(physician));
        }

        private async Task DeletePhysician(Physician physician)
        {
            if (physician == null)
                return;

            bool confirm = await App.Current.MainPage.DisplayAlert("Confirm", $"Delete {physician.Display}?", "Yes", "No");

            if (confirm)
            {
                ChartServiceProxy.Current.RemovePhysician(physician);
                NotifyPropertyChanged(nameof(Physicians));
            }
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
