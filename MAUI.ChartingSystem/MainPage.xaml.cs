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

        }
    }
}
