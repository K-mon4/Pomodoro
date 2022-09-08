using System.ComponentModel;
namespace Pomodoro.Models;

public class Todo : INotifyPropertyChanged
{
	public string TodoName
	{
		get;
		set;
	}

	public DateTime? LastDone { get; set; }

	public Todo(string name)
	{
		TodoName = name;
	}

	public event PropertyChangedEventHandler PropertyChanged;
}
