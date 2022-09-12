using Pomodoro.ViewModels;

namespace Pomodoro.Views;

public partial class LogPage : ContentPage
{
	public  LogPageViewModel logPageViewModel = new LogPageViewModel();
	public LogPage()
	{
		InitializeComponent();
		logPageViewModel.setLogsList();

		Console.Write("");
		BindingContext = logPageViewModel;

	}
}
