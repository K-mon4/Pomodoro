
using Pomodoro.ViewModels;
namespace Pomodoro.Views;

public partial class HomePage : ContentPage
{
    HomePageViewModel homePageViewModel = new HomePageViewModel();
	public HomePage()
	{
		InitializeComponent();
        BindingContext = homePageViewModel;
        startBtn.Clicked += homePageViewModel.createTimerPage;
	}

    void tasksCollection_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
    }



    void OnPickerSelectedIndexChanged()
    {

    }

    void todoCollection_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
    }
}
