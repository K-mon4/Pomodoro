using System.ComponentModel;
namespace Pomodoro.Models;

public class Todo : INotifyPropertyChanged
{
	private string _todoname;
	public string TodoName
	{
		get => _todoname;
		set
		{
			_todoname = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TodoName)));
		}
	}

	public DateTime? LastDone { get; set; }
	public DateTime CreatedTime { get; set; }
	public int TotalTime { get; set; }
	public Todo(string name)
	{
		TodoName = name;
	}

	#nullable enable
    public Todo(string name, string created, string? lastdone, int totaltime)
	{

		if(string.IsNullOrEmpty(lastdone))
		{
			LastDone = null;
        }
		else
		{
            LastDone = DateTime.ParseExact(lastdone, "yyyy-MM-dd HH:mm:ss.FFFFFFF", System.Globalization.CultureInfo.CurrentCulture);
        }
		TodoName = name;
		
		CreatedTime = DateTime.ParseExact(created, "yyyy-MM-dd HH:mm:ss.FFFFFFF", System.Globalization.CultureInfo.CurrentCulture);
		TotalTime = totaltime;
    }
	#nullable disable

	public static Todo todoDefault = new Todo("頑張る");
	public event PropertyChangedEventHandler PropertyChanged;
}
