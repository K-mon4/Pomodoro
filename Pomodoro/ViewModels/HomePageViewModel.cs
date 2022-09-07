using System.Collections.ObjectModel;
using System.ComponentModel;
using Pomodoro.Models;

namespace Pomodoro.ViewModels;

public class HomePageViewModel : INotifyPropertyChanged
{
	
	ObservableCollection<Todo> TodoList { get; set; } = new ObservableCollection<Todo>();

	public Todo _todoSelected = new Todo { name = "Do", lastDone = null };
	public Todo TodoSelected
	{
		get => _todoSelected;

		set
		{
			_todoSelected = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TodoSelected)));
		}
	}

	public async void createTimerPage(Object sender, EventArgs e)
	{
		await AppShell.Current.GoToAsync($"{nameof(Views.TimerPage)}?{nameof(Views.TimerPage.TodoName)}={this.TodoSelected.name}");
		//await AppShell.Current.GoToAsync($"{nameof(Views.TimerPage)}?TodoName=OraOraOraOra");
    }
	public HomePageViewModel()
	{
		
	}

	public event PropertyChangedEventHandler PropertyChanged;

}
