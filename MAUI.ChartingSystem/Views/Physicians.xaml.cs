using MAUI.ChartingSystem.ViewModels;

namespace MAUI.ChartingSystem.Views;

public partial class Physicians : ContentPage
{
	private PhysiciansViewModel? _viewModel;
	public Physicians()
	{
		InitializeComponent();
	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
		_viewModel = new PhysiciansViewModel();
		BindingContext = _viewModel;
    }

    private async void BackClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//MainPage");
    }

    private async void AddClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Physician?physicianId=0");
    }
}