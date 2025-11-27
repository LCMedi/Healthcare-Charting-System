using MAUI.ChartingSystem.ViewModels;

namespace MAUI.ChartingSystem.Views;

public partial class Patients : ContentPage
{
	private PatientsViewModel? _viewModel;
	public Patients()
	{
		InitializeComponent();
	}

    /*private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
		_viewModel = new PatientsViewModel();
		BindingContext = _viewModel;
    }*/

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (BindingContext is not PatientsViewModel vm)
        {
            _viewModel = new PatientsViewModel();
            BindingContext = _viewModel;
            vm = _viewModel;
        }

        await vm.LoadPatients();
    }

    private async void BackClicked(object sender, EventArgs e)
    {
		await Shell.Current.GoToAsync("//MainPage");
    }

    private async void AddClicked(object sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//Patient?patientId=0");
    }

    private async void SearchBar_SearchButtonPressed(object sender, EventArgs e)
    {
        if (BindingContext is PatientsViewModel vm)
        {
            await vm.ExecuteSearchAsync();
        }
    }
}