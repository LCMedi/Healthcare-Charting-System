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

        public ICommand AddSpecCommand { get; set; }

        public AddPhysicianViewModel()
        {
            AddSpecCommand = new Command(AddSpec);
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

        public AddPhysicianViewModel(int id) : this()
        {
            var physician = ChartServiceProxy.Current.GetPhysician(id);
            if (physician is null)
                return;

            Name = physician.Name ?? string.Empty;
            LicenseNumber = physician.LicenseNumber ?? string.Empty;
            GraduationDate = physician.graduationDate ?? DateTime.Today;
            Specializations = physician.Specializations ?? string.Empty;
        }


        public void Reset()
        {
            Name = string.Empty;
            LicenseNumber = string.Empty;
            GraduationDate = DateTime.Today;
            Specializations = string.Empty;
        }

        private Physician MakePhysician()
        {
            var physician = new Physician(Name, LicenseNumber, GraduationDate, Specializations);
            return physician;
        }

        public bool CreatePhysician(out Physician? physician, out string error)
        {
            try
            {
                physician = MakePhysician();
                error = string.Empty;
                return true;
            }
            catch (ArgumentException ex)
            {
                physician = null;
                error = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                physician = null;
                error = ex.Message;
                return false;
            }
        }

        public bool UpdatePatient(Physician existing, out string error)
        {
            try
            {
                existing.SetName(Name);
                existing.SetLicenseNumber(LicenseNumber);
                existing.SetGraduationDate(GraduationDate);
                existing.Specializations = Specializations;
                error = string.Empty;
                return true;
            }
            catch (ArgumentException ex)
            {
                error = ex.Message;
                return false;
            }
            catch (Exception ex)
            {
                error = ex.Message;
                return false;
            }
        }
    }
}
