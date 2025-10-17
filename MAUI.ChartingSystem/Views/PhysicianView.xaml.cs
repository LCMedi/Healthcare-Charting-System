using Library.ChartingSystem.Models;
using Library.ChartingSystem.Services;
using MAUI.ChartingSystem.ViewModels;

namespace MAUI.ChartingSystem.Views;

[QueryProperty(nameof(PhysicianId), "physicianId")]
public partial class PhysicianView : ContentPage
{
    private PhysicianViewModel? _viewModel;
    public int PhysicianId { get; set; }
    public PhysicianView()
	{
		InitializeComponent();
	}

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if (PhysicianId > 0)
        {
            _viewModel = new PhysicianViewModel(PhysicianId);
            BindingContext = _viewModel;
        }
        else
        {
            if (_viewModel is null)
            {
                _viewModel = new PhysicianViewModel();
                BindingContext = _viewModel;
            }
            else
            {
                _viewModel.Reset();
            }
        }
    }

    private async void OkClicked(object sender, EventArgs e)
    {
        if (_viewModel is null)
            return;
        if (PhysicianId <= 0)
        {
            if (!_viewModel.CreatePhysician(out var physician, out var error))
            {
                await DisplayAlert("Invalid data", error, "OK");
                return;
            }
            ChartServiceProxy.Current.AddPhysician(physician!);
        }


        await Shell.Current.GoToAsync("//MainPage");
    }

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }
}