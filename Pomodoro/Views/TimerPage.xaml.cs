using Pomodoro.ViewModels;
using System.ComponentModel;
namespace Pomodoro.Views;

[QueryProperty(nameof(TodoName), "TodoName")]
public partial class TimerPage : ContentPage
{


    public TimerPageViewModel timerPageViewModel = new TimerPageViewModel();
    public string _todoname;
    public string TodoName
    {
        get => _todoname;
        set
        {
            _todoname = value;
            timerPageViewModel.TodoName = _todoname;
            OnPropertyChanged();
        }
    }
    public TimerPage()
	{
        InitializeComponent();

        BindingContext = timerPageViewModel;
        timerAddBtn.Clicked += timerPageViewModel.TimerAdd;
        timerSubBtn.Clicked += timerPageViewModel.TimerSubtract;
        timerControllBtn.Clicked += timerPageViewModel.ControllBtn_Clicked;
    }

    


}
