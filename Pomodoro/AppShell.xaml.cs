namespace Pomodoro;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(Views.TimerPage), typeof(Views.TimerPage));
	}
}

