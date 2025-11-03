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

        private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
        {
        }

        private async void PatientsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Patients");
        }

        private async void PhysiciansClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Physicians");
        }

        private async void AppointmentsClicked(object sender, EventArgs e)
        {
            await Shell.Current.GoToAsync("//Appointments");
        }
    }
}
