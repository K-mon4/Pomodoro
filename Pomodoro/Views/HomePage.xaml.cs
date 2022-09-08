using CommunityToolkit.Maui.Views;
using Pomodoro.Models;
using Pomodoro.ViewModels;


namespace Pomodoro.Views;



public partial class HomePage : ContentPage
{
    HomePageViewModel homePageViewModel = new HomePageViewModel();
	public HomePage()
	{
		InitializeComponent();
        BindingContext = homePageViewModel;
        startBtn.Clicked += homePageViewModel.createTimerPage;

        todoSelectBtn.Clicked += todoSelectBtnClicked;
	}


    public async void todoSelectBtnClicked(Object s, EventArgs e)
    {
        var selectMenuPop = new todoSelectorPopup();
        selectMenuPop.BindingContext = homePageViewModel;

        var result = await this.ShowPopupAsync(selectMenuPop);

        if(result is Todo todoResult)
        {
            this.todoSelectBtn.Text = todoResult.TodoName;
            homePageViewModel.TodoSelected = todoResult;
        }
    }

    void todoCollection_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
    }

}
