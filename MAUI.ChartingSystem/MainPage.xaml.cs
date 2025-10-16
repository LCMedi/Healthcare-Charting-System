using System.ComponentModel;
using System.Reflection.Metadata;
using MAUI.ChartingSystem.ViewModels;

namespace MAUI.ChartingSystem
{
    public partial class MainPage : ContentPage
    {

        public MainPage()
        {
            InitializeComponent();
            BindingContext = new MainViewModel();
        }

        private void AddPatientClicked(object sender, EventArgs e)
        {
            Shell.Current.GoToAsync("//Patient?patientId=0");
        }

        private void EditPatientClicked(object sender, EventArgs e)
        {
            var selectedId = (BindingContext as MainViewModel)?.SelectedPatient?.Id ?? 0;
            Shell.Current.GoToAsync($"//Patient?patientId={selectedId}");
        }

        private void DeletePatientClicked(object sender, EventArgs e)
        {
            (BindingContext as MainViewModel)?.Delete();
        }

        private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
            (BindingContext as MainViewModel)?.Refresh();
        }

        private void EditPhysiciansClicked(object sender, EventArgs e)
        {

        }

        private void DeletePhysiciansClicked(object sender, EventArgs e)
        {

        }

        private void EditAppointmentClicked(object sender, EventArgs e)
        {

        }

        private void DeleteAppointmentClicked(object sender, EventArgs e)
        {

        }
    }
}
