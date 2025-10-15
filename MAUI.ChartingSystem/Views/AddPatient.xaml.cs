using Library.ChartingSystem.Services;

namespace MAUI.ChartingSystem.Views;

public partial class PatientView : ContentPage
{
	public PatientView()
	{
		InitializeComponent();

	}

    private void CancelClicked(object sender, EventArgs e)
    {
		Shell.Current.GoToAsync("//MainPage");
    }

    private void OkClicked(object sender, EventArgs e)
    {
        // add the patient
        
    }
}