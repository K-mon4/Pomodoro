using Pomodoro.Models;
using Pomodoro.Views;
using System.ComponentModel;
using System.Collections.ObjectModel;

namespace Pomodoro.ViewModels;

public class LogsCollection : INotifyPropertyChanged
{
	private DateTime _day;
	public DateTime Day {
		get => _day;
		set
		{
			_day = value;
			DayText = value.ToString();
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DayText)));
		}
	}
	private string _daytext;
	public string DayText
	{
		get => _daytext;
		set
		{
			_daytext = value;
			
		}
	}
	private ObservableCollection<todoLog> _daylogs;
	public ObservableCollection<todoLog> DayLogs
	{
		get => _daylogs;
		set
		{
			_daylogs = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(DayLogs)));
		}
	}
	public LogsCollection(IEnumerable<todoLog> daylogs, DateTime day)
	{
		this.Day = day;
		this.DayLogs = new ObservableCollection<todoLog>(daylogs);
	}

	public event PropertyChangedEventHandler PropertyChanged;
}
public class LogPageViewModel :INotifyPropertyChanged
{

	databaseIO dbio = new databaseIO();
	public LogPageViewModel()
	{

	}


	//こっから複雑
	private ObservableCollection<LogsCollection> _logslist;
        
        public ObservableCollection<LogsCollection> LogsList
	{
		get => _logslist;
		set
		{
			_logslist = value;
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LogsList)));
		}
	}

	public void setLogsList()
	{
		List<todoLog> rawloglist = dbio.getLogs();
		List<DateTime> days;

		var daysenum = (from u in rawloglist
						select
						DateTime.ParseExact(u.StartedTime, "yyyy-MM-dd HH:mm:ss.FFFFFFF", null)).DistinctBy(
						u => u.Date
						);

		LogsList = new ObservableCollection<LogsCollection>();

		days = new List<DateTime>(daysenum);
		foreach(DateTime day in days)
		{
			var logcole =
					from u in rawloglist
					where DateTime.ParseExact(u.StartedTime, "yyyy-MM-dd HH:mm:ss.FFFFFFF", null).Date == day.Date
					orderby DateTime.ParseExact(u.StartedTime, "yyyy-MM-dd HH:mm:ss.FFFFFFF", null) descending
					select u;

            LogsList.Add(
				new LogsCollection(logcole,
					day.Date
                        ));
		}

    }

	public void MakeCollectionview()
	{
		
		
	}

	public event PropertyChangedEventHandler PropertyChanged;
    }

