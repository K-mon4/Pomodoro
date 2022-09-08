
namespace Pomodoro.Models;

public class Todo 
{
	
	public string TodoName { get; set; }

	public DateTime? LastDone { get; set; }

	public Todo(string name)
	{
		TodoName = name;
	}
}
