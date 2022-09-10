using System.Collections.ObjectModel;
using System.ComponentModel;
using Pomodoro.Models;
using Pomodoro.Views;
using CommunityToolkit.Maui.Views;
using System.Windows.Input;

namespace Pomodoro.ViewModels;




public class HomePageViewModel : INotifyPropertyChanged
{

	TodoListFunctions todolf = new TodoListFunctions();

	private ObservableCollection<Todo> _todolist;
    public ObservableCollection<Todo> TodoList
	{
		get => _todolist;
		set
		{
			_todolist = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TodoList)));
		}
	}

	//public string _todoselectedText;
	//public string TodoSelectedText
	//{
	//	get => _todoselectedText;
	//	set
	//	{
	//		_todoselectedText = value;
	//		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TodoSelectedText)));
	//	}
	//}

    private Todo _todoSelected;

	public Todo TodoSelected
	{
		get => _todoSelected;

		set
		{
			if(value != null)
			{
                _todoSelected = value;
                
            }
			else
			{
				_todoSelected = Todo.todoDefault;
			}
			//TodoSelectedText = _todoSelected.TodoName;
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
		if (TodoList.FirstOrDefault() != null)
		{
			TodoSelected = TodoList.FirstOrDefault();
		}
		else
		{
			TodoSelected = Todo.todoDefault;
		}

		DeleteCommand = new Command<Todo>(
			execute: (Todo todo) =>
			{
				//TodoList.Remove(todo);
				todolf.Deletefromdb(todo.TodoName);
				TodoList = new  ObservableCollection<Todo>(todolf.GetTodoList());
				if(TodoSelected == todo)
				{
					if(TodoList.FirstOrDefault() != null)
					{
						TodoSelected = TodoList.FirstOrDefault();
					}
					else
					{
						TodoSelected = Todo.todoDefault;
					}
					//TodoSelectedText = TodoSelected.TodoName;
				}
			},
			canExecute: (Todo todo) =>
			{

				return true;
			}
			);
	}

	public ICommand DeleteCommand { get; private set; }

    public async void AddCommand(Object Sender, EventArgs e)
	{

		string todoname = await AppShell.Current.DisplayPromptAsync("Enter Task name", null, cancel:"Cancel");
		if(todoname == null)
		{
			return;
		}
		foreach(Todo todo in TodoList)
		{
			if(todo.TodoName == todoname)
			{
				// Duplicate
				return;
			}
		}

		// TodoList.Add(new Todo(todoname));
		todolf.Addtodb(todoname);
		TodoList = new ObservableCollection<Todo>(todolf.GetTodoList());
		return;
		
	}

    public event PropertyChangedEventHandler PropertyChanged;

}
