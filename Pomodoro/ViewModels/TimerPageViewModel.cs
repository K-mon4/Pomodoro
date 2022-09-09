using System.ComponentModel;
using Pomodoro.Models;

namespace Pomodoro.ViewModels;

// state to save to json
// timer started  timer ended to do Name

public enum TimerMode
{
	Pomodoro,
	ShortBreak,
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

public enum TimerRequest
{
	NextStep,
	Exit,
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
	public bool SaveStateJson()
	{
		// TimerStarted, Timer Ended(DateTime.Now), Name
		return true;
	}

	// TaskCompletionSource<TimerEvent> tcs = new TaskCompletionSource<TimerEvent>();

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
	public string StartedTimeText
	{
		get => _startedtimeText;
		set
		{
			_startedtimeText = value;
		}
	}


    private DateTime _expectedEndTime;
	public DateTime ExpectedEndtime
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
	public string ExpectedEndTimeText
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
			
			//if(timerState != null)
			//{
			//	ExpectedEndtime = StartedTime + _timeMeasuring;
			//}
			if(TimeMeasuring > TimeSpan.FromMinutes(60))
			{
                this.TimeMeasuringText = _timeMeasuring.ToString("hh':'mm");

            }
			else
			{
                this.TimeMeasuringText = _timeMeasuring.ToString("mm");
            }
            
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
				if(TimerValue > TimeSpan.FromMinutes(60))
				{
                    TimerValueText = $"{this.TimeMeasuringText} + {(DateTime.Now - ExpectedEndtime).ToString("hh':'mm':'ss")}";
                }
				else
				{
                    TimerValueText = $"{this.TimeMeasuringText} + {(DateTime.Now - ExpectedEndtime).ToString("'mm':'ss")}";
                }
				
			}
			else
			{
                if (TimerValue > TimeSpan.FromMinutes(60))
                {
                    TimerValueText = timervalue.ToString("hh':'mm':'ss");
                }
                else
                {
                    TimerValueText = timervalue.ToString("mm':'ss");
                }
            }
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("TimerValueText"));
		}
	}

	public TimerMode timerMode;
	public TimerDurations timerDurations;

	public TimerState timerState;

	bool discardRequest = false;
	bool completeRequest = false;



    public TimerPageViewModel()
	{
		timerDurations = new TimerDurations();
		StartTimer();
	}

	// set a timer for pomodoro and start it automatically
	public async void StartTimer()
	{
		//
		timerMode = TimerMode.Pomodoro;

		// set timemeasuring
		while(true)
		{
            InitializeTimer();

            TimerRequest result = await StartTimerLoop();

			if(result == TimerRequest.Exit)
			{
				await AppShell.Current.GoToAsync("..");
			}
			else
			{
				if(this.timerMode == TimerMode.Pomodoro)
				{
					this.timerMode = TimerMode.ShortBreak;
				}
				else
				{
					this.timerMode = TimerMode.Pomodoro;
				}
			}
        }
		
	}

	private void InitializeTimer()
	{

		switch(this.timerMode)
		{
			case TimerMode.Pomodoro:
				this.TimeMeasuring = this.timerDurations.Pomodoro;
				break;
			case TimerMode.ShortBreak:
				this.TimeMeasuring = this.timerDurations.ShortBreak;
				break;
			default:
				break;
		}
	}

	private async Task<TimerRequest> StartTimerLoop()
	{

		TimeSpan timeleft = this.TimeMeasuring;
		StartedTime = DateTime.Now;
		ExpectedEndtime = StartedTime + this.TimeMeasuring;
		TimeSpan timepast = TimeSpan.Zero;

		//
        timerState = TimerState.Counting;


        this.TimerValue = timeleft;

		while(true)
		{
				
            if (discardRequest)
			{
				discardRequest = false;
				return TimerRequest.Exit;
				// exit timer view
				//await Shell.Current.GoToAsync("..");
			}
			if(completeRequest)
			{
				completeRequest = false;
				return TimerRequest.NextStep;
				// Save log to json file
				// Started time, finished time,
			}

            if (this.ExpectedEndtime < DateTime.Now)
            {
                this.timerState = TimerState.Overtime;


            }
			else
			{
				this.timerState = TimerState.Counting;
                // count the time
				timepast = DateTime.Now - StartedTime;
                timeleft = this.TimeMeasuring - timepast;

                // update view
                this.TimerValue = timeleft; // this updates timervaluetext automatically
            }
                

            await Task.Delay(100);
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
	public async void ControllBtn_Clicked(Object sender, EventArgs e)
	{
        if (this.timerMode == TimerMode.Pomodoro)
        {
            if (this.timerState == TimerState.Counting)
            {
				// Pomodoro Counting
				string request = await AppShell.Current.DisplayActionSheet(null,"Cancel", null, "Discard this session", "Complete earlier");
				if(request == "Discard this session")
				{
					this.discardRequest = true;
				}
				else if(request == "Complete earlier")
				{
					this.completeRequest = true;
				}
				else
				{

				}
            }
            else
            {
				// Pomodoro Overtime
				// Complete session
				this.completeRequest = true;

            }
        }
        else
        {
            if (this.timerState == TimerState.Counting)
            {
				// ShortBreak Counting
				// Skip time left and star pomodoro timer
				this.completeRequest = true;
            }
			else
			{
				this.completeRequest = true;

			}
        }
    }


	// TODO 改良の余地あり
	public void TimerAdd(Object s, EventArgs e)
	{
		TimeMeasuring += TimeSpan.FromMinutes(5);
		TimerValue += TimeSpan.FromMinutes(5);
		this.ExpectedEndtime = StartedTime + TimeMeasuring;

    }
	public void TimerSubtract(Object s, EventArgs e)
	{

		if(TimeMeasuring > TimeSpan.Zero && TimeMeasuring <= TimeSpan.FromMinutes(1))
		{
			// do nothing
		}
		else if(TimeMeasuring > TimeSpan.FromMinutes(1) && TimeMeasuring < TimeSpan.FromMinutes(5))
		{
			TimeMeasuring = TimeSpan.FromMinutes(1);
			
		}
		else
		{
			TimeMeasuring -= TimeSpan.FromMinutes(5);
		}

        this.ExpectedEndtime = StartedTime + TimeMeasuring;
    }

	public event PropertyChangedEventHandler PropertyChanged;
}
