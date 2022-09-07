using System.ComponentModel;
using Pomodoro.Models;

namespace Pomodoro.ViewModels;


public enum TimerMode
{
	Pomodoro = 0,
	ShortBreak = 1,
}
public struct TimerDurations
{
	public TimeSpan Pomodoro;
	public TimeSpan ShortBreak;
	public TimerDurations()
	{
		Pomodoro = TimeSpan.FromMinutes(25);
		ShortBreak = TimeSpan.FromMinutes(5);
    }
}

public enum TimerState
{
	Counting,
	Overtime,
}

public enum TimerEvent
{
	NextStep,
	Exit,
	TimeAdd,
}

public class TimerPageViewModel : INotifyPropertyChanged
{
	/// <summary>
	///
	/// property for view
	///		TimerValueText
	///		TimeMeasuring
	///		TimerStarted
	///	method for view
	///		AddTimer
	///		SubTimer
	/// </summary>

	JsonIO jsoninterface = new JsonIO();
	TaskCompletionSource<TimerEvent> tcs = new TaskCompletionSource<TimerEvent>();

	// Property Definition
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

	private DateTime _startedtime;
	public DateTime StartedTime
	{
		get => _startedtime;
		set
		{
			_startedtime = value;
			StartedTimeText = _startedtime.ToString("hh:mm");
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(StartedTimeText)));
		}
	}
	private string _startedtimeText;
	private string StartedTimeText
	{
		get => _startedtimeText;
		set
		{
			_startedtimeText = value;
		}
	}


    private DateTime _expectedEndTime;
	private DateTime ExpectedEndtime
	{
		get => _expectedEndTime;
		set
		{
			_expectedEndTime = value;
			ExpectedEndTimeText = _expectedEndTime.ToString("hh:mm");
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ExpectedEndTimeText)));
		}
	}
	private string _expectedEndTimeText;
	private string ExpectedEndTimeText
	{
		get => _expectedEndTimeText;
		set
		{
			_expectedEndTimeText = value;
		}
	}

    private TimeSpan _timeMeasuring;
	public TimeSpan TimeMeasuring
	{
		get => _timeMeasuring;
		set
		{
			_timeMeasuring = value;
			
			if(timerState != null)
			{
				ExpectedEndtime = StartedTime + _timeMeasuring;
			}
			this.TimeMeasuringText = _timeMeasuring.ToString("mm");
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TimeMeasuringText)));
		}
	}

	// binded to view
	public string _timeMeasuringText;
	public string TimeMeasuringText
	{
		get => _timeMeasuringText;
		set
		{
			_timeMeasuringText = value;
		}
	}


	// binded to view
	private string _timervaluetext;
    public string TimerValueText
	{
		get => _timervaluetext;
		set
		{
			_timervaluetext = value;
		}
	}

	//Timer value 
    private TimeSpan timervalue;
	public TimeSpan TimerValue
	{
		get
		{
			return timervalue;
		}
		set
		{
			timervalue = value;
			if (this.timerState == TimerState.Overtime)
			{
				TimerValueText = $"{this.TimeMeasuringText} + {(DateTime.Now - ExpectedEndtime).ToString("mm':'ss")}";
			}
			else
			{
				TimerValueText = timervalue.ToString("mm':'ss");
			}
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TimerValueText"));
		}
	}

	public TimerMode timerMode;
	public TimerDurations timerDurations;

	public TimerState timerState;

	bool discardRequest = false;
	bool completeEarlierRequest = false;



    public TimerPageViewModel()
	{
		timerDurations = new TimerDurations();
		StartTimer();
	}

	public void StartTimer()
	{
		
		timerMode = TimerMode.Pomodoro;

		// set initial timer value and time measuring
		//
		// InitializeTimer();
		
		StartTimerLoop();
	}

	private void InitializeTimer()
	{
		timerState = TimerState.Counting;

		switch(this.timerMode)
		{
			case TimerMode.Pomodoro:
				this.TimeMeasuring = this.timerDurations.Pomodoro;
				this.TimerValue = this.timerDurations.Pomodoro;
				break;
			case TimerMode.ShortBreak:
				this.TimeMeasuring = this.timerDurations.ShortBreak;
				this.TimerValue = this.timerDurations.ShortBreak;
				break;
			default:
				break;
		}
	}

	private async void StartTimerLoop()
	{

		TimeSpan timeleft = this.TimeMeasuring;
		DateTime timerstarted = DateTime.Now;
		DateTime expectedEndtime = timerstarted += this.TimeMeasuring;
		TimeSpan timepast = TimeSpan.Zero;


		while(true)
		{
			while(true)
			{
				
                if (discardRequest)
				{
					// exit timer view
					await Shell.Current.GoToAsync("..");
				}
				if(completeEarlierRequest)
				{
					// Save log to json file
					// Started time, finished time,
					jsoninterface.SavePlogToJson(timerstarted, DateTime.Now, TodoName);
					break;
				}
                if (this.ExpectedEndtime > DateTime.Now)
                {
                    this.timerState = TimerState.Overtime;


                }
				else
				{
					this.timerState = TimerState.Counting;
                    // count the time
					timepast = DateTime.Now - timerstarted;
                    timeleft = this.TimeMeasuring - timepast;

                    // update view
                    this.TimerValue = timeleft; // this updates timervaluetext automatically
                }
                

                await Task.Delay(100);
            }
        }
	}
	
	// TODO
	public void ControllBtnUpdate()
	{
		if(this.timerMode == TimerMode.Pomodoro)
		{
			if(this.timerState == TimerState.Counting)
			{

			}
			else
			{

			}
		}
		else
		{
			if(this.timerState == TimerState.Counting)
			{

			}
		}
	}
	// TODO
	public void ControllBtn_Clicked(Object sender, EventArgs e)
	{
        if (this.timerMode == TimerMode.Pomodoro)
        {
            if (this.timerState == TimerState.Counting)
            {

            }
            else
            {

            }
        }
        else
        {
            if (this.timerState == TimerState.Counting)
            {

            }
        }
    }

	// TODO 改良の余地あり
	public void TimerAdd(Object s, EventArgs e)
	{
		TimeMeasuring += TimeSpan.FromMinutes(5);
		TimerValue += TimeSpan.FromMinutes(5);
	}
	public void TimerSubtract(Object s, EventArgs e)
	{
		TimeMeasuring -= TimeSpan.FromMinutes(5);
		TimerValue -= TimeSpan.FromMinutes(5);
	}

	public event PropertyChangedEventHandler PropertyChanged;
}
