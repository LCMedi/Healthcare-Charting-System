using Library.ChartingSystem.Models;
using Library.ChartingSystem.Services;
using MAUI.ChartingSystem.ViewModels;

namespace MAUI.ChartingSystem.Views;

[QueryProperty(nameof(PhysicianId), "physicianId")]
public partial class PhysicianView : ContentPage
{
    private AddPhysicianViewModel? _viewModel;
    public int PhysicianId { get; set; }
    public PhysicianView()
	{
		InitializeComponent();
	}
    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//Physicians");
    }
    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        // If updating physician
        if (PhysicianId > 0)
        {
            _viewModel = new AddPhysicianViewModel(PhysicianId);
            BindingContext = _viewModel;
        }
        // If creating physician
        else
        {
            if (_viewModel is null)
            {
                _viewModel = new AddPhysicianViewModel();
                BindingContext = _viewModel;
            }
            else
            {
                _viewModel.Reset();
            }
        }
    }
}