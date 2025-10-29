using MAUI.ChartingSystem.ViewModels;

namespace MAUI.ChartingSystem.Views;

public partial class Appointments : ContentPage
{
	private AppointmentsViewModel? _viewModel;
	public Appointments()
	{
		InitializeComponent();
	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if (_viewModel is null)
        {
		    _viewModel = new AppointmentsViewModel();
		    BindingContext = _viewModel;
        }
        else
        {
            _viewModel.Refresh();
        }
    }

    private async void BackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void AddClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Appointment?appointmentId=0");
    }
}