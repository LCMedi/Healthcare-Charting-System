using Library.ChartingSystem.Models;
using Library.ChartingSystem.Services;
using Microsoft.Maui.Controls;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace MAUI.ChartingSystem.ViewModels
{
    public class AddPhysicianViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        private Physician? _physician = null;

        private string _name = string.Empty;
        private string _licenseNumber = string.Empty;
        private DateTime _graduationDate = DateTime.Now;
        private string _specializations = string.Empty;
        private string _newSpec = string.Empty;
        
        public string Name
        {
            get { return _name; }
            set { _name = value; OnPropertyChanged(); }
        }

        public string LicenseNumber
        {
            get { return _licenseNumber; }
            set { _licenseNumber = value; OnPropertyChanged(); }
        }
        public DateTime GraduationDate
        {
            get { return _graduationDate; }
            set { _graduationDate = value; OnPropertyChanged(); }
        }

        public string Specializations
        {
            get { return _specializations; }
            set { _specializations = value; OnPropertyChanged(); }
        }

        public string NewSpec
        {
            get { return _newSpec; }
            set { _newSpec = value; OnPropertyChanged(); }
        }

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        public ICommand? AddSpecCommand { get; set; }
        public ICommand? SavePhysicianCommand { get; set; }

        // Constructor for Creates
        public AddPhysicianViewModel()
        {
            SetUpCommands();
        }

        // Constructor for Updates
        public AddPhysicianViewModel(int id) : this()
        {
            _physician = ChartServiceProxy.Current.GetPhysician(id);
            if (_physician == null)
                return;

            Name = _physician.Name ?? string.Empty;
            LicenseNumber = _physician.LicenseNumber ?? string.Empty;
            GraduationDate = _physician.graduationDate ?? DateTime.Today;
            Specializations = _physician.Specializations ?? string.Empty;

            SetUpCommands();
        }

        private void SetUpCommands()
        {
            AddSpecCommand = new Command(_ => AddSpec());
            SavePhysicianCommand = new Command(async _ => await DoSave());
        }

        private async Task DoSave()
        {
            try
            {
                // If updating
                if (_physician != null)
                {
                    // Try updating physician
                    _physician.SetName(Name);
                    _physician.SetLicenseNumber(LicenseNumber);
                    _physician.SetGraduationDate(GraduationDate);
                    _physician.Specializations = Specializations;
                    await Shell.Current.DisplayAlert("Success", "Physician updated successfully!", "OK");
                    await Shell.Current.GoToAsync("//Physicians");
                }
                // If creating
                else
                {
                    // Try adding physician
                    ChartServiceProxy.Current.AddPhysician(new Physician(Name, LicenseNumber, GraduationDate, Specializations));
                    await Shell.Current.DisplayAlert("Success", "Physician added successfully!", "OK");
                    await Shell.Current.GoToAsync("//Physicians");
                }
            }
            catch (ArgumentException ex)
            {
                await Shell.Current.DisplayAlert("Error", ex.Message, "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Something went wrong: {ex.Message}", "OK");
            }
        }
        public void AddSpec()
        {
            var spec = NewSpec?.Trim();
            if (string.IsNullOrWhiteSpace(spec))
                return;

            if (string.IsNullOrWhiteSpace(Specializations))
                Specializations = spec;
            else
                Specializations = $"{Specializations}, {spec}";

            NewSpec = string.Empty;
        }

        public void Reset()
        {
            _physician = null;
            Name = string.Empty;
            LicenseNumber = string.Empty;
            GraduationDate = DateTime.Today;
            Specializations = string.Empty;
        }
    }
}
