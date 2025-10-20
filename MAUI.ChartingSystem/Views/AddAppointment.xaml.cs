using Library.ChartingSystem.Models;
using Library.ChartingSystem.Services;
using MAUI.ChartingSystem.ViewModels;

namespace MAUI.ChartingSystem.Views;

[QueryProperty(nameof(AppointmentId), "appointmentId")]
public partial class AppointmentView : ContentPage
{
	private AddAppointmentViewModel? _viewModel;
    public int AppointmentId { get; set; }

    public AppointmentView()
	{
		InitializeComponent();
	}

    private void CancelClicked(object sender, EventArgs e)
    {
        Shell.Current.GoToAsync("//MainPage");
    }

    private void ContentPage_NavigatedTo(object sender, NavigatedToEventArgs e)
    {
        if (AppointmentId == 0)
        {
            _viewModel = new AddAppointmentViewModel();
            BindingContext = _viewModel;
        } 
        else
        {
            if (_viewModel == null)
            {
                _viewModel = new AddAppointmentViewModel(AppointmentId);
                BindingContext = _viewModel;
            } 
            else
            {
                _viewModel.Reset();
            }
        }
    }
}