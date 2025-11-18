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

        public ObservableCollection<Physician> AllPhysicians { get; set; } = new();

        public ObservableCollection<Physician> FilteredPhysicians { get; set; } = new();

        private string _physicianSearchText;
        public string PhysicianSearchText
        {
            get => _physicianSearchText;
            set
            {
                if (_physicianSearchText != value)
                {
                    _physicianSearchText = value;
                    NotifyPropertyChanged();
                    FilterPhysicians();
                }
            }
        }

        public PhysiciansViewModel()
        {
            LoadPhysicians();
            SetUpCommands();
        }

        public void LoadPhysicians()
        {
            AllPhysicians.Clear();
            var data = ChartServiceProxy.Current.GetAllPhysicians();

            foreach (var p in data)
                AllPhysicians.Add(p);

            FilterPhysicians();
        }



        public void Refresh()
        {
            LoadPhysicians();
        }

        public ICommand? DeletePhysicianCommand { get; set; }
        public ICommand? EditPhysicianCommand { get; set; }

        private void SetUpCommands()
        {
            DeletePhysicianCommand = new Command<Physician>(async (physician) => await DeletePhysician(physician));
            EditPhysicianCommand = new Command<Physician>(async (physician) => await EditPhysician(physician));
        }
        private void FilterPhysicians()
        {
            string query = PhysicianSearchText?.ToLower() ?? "";

            var results = AllPhysicians
                .Where(p => string.IsNullOrEmpty(query)
                || p.Name.ToLower().Contains(query))
                .ToList();

            FilteredPhysicians.Clear();

            foreach (var p in results)
                FilteredPhysicians.Add(p);
        }
        private async Task DeletePhysician(Physician physician)
        {
            if (physician == null)
                return;

            bool confirm = await App.Current.MainPage.DisplayAlert("Confirm", $"Delete {physician.Display}?", "Yes", "No");

            if (confirm)
            {
                ChartServiceProxy.Current.RemovePhysician(physician);
                Refresh();
            }
        }

        private async Task EditPhysician(Physician physician)
        {
            if (physician == null)
                return;

            await Shell.Current.GoToAsync($"//Physician?physicianId={physician.Id}");
        }

        private void NotifyPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
