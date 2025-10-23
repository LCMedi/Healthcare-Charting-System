using MAUI.ChartingSystem.ViewModels;

namespace MAUI.ChartingSystem.Views;

public partial class Patients : ContentPage
{
	private PatientsViewModel? _viewModel;
	public Patients()
	{
		InitializeComponent();
	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
		_viewModel = new PatientsViewModel();
		BindingContext = _viewModel;
    }

    private async void BackClicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("//MainPage");
    }

    private async void AddClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Patient?patientId=0");
    }
}