using System.Collections.ObjectModel;
using System.ComponentModel;
using Pomodoro.Models;
using Pomodoro.Views;
using CommunityToolkit.Maui.Views;


namespace Pomodoro.ViewModels;




public class HomePageViewModel : INotifyPropertyChanged
{

	TodoListFunctions todolf = new TodoListFunctions();

    public ObservableCollection<Todo> TodoList { get; set; } = new ObservableCollection<Todo>();

	private Todo _todoSelected = new Todo("Do");
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
		await AppShell.Current.GoToAsync($"{nameof(Views.TimerPage)}?{nameof(Views.TimerPage.TodoName)}={this.TodoSelected.TodoName}");
    }
	public HomePageViewModel()
	{
		
		TodoList = new ObservableCollection<Todo>(todolf.GetTodoList());
	}

    

    public event PropertyChangedEventHandler PropertyChanged;

}
