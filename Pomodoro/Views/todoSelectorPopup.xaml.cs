using Pomodoro.Models;
using CommunityToolkit.Maui.Views;


namespace Pomodoro.Views;

public partial class todoSelectorPopup : Popup
{
	public todoSelectorPopup()
	{
		InitializeComponent();
	}

    void CollectionView_SelectionChanged(System.Object sender, Microsoft.Maui.Controls.SelectionChangedEventArgs e)
    {
		Close((e.CurrentSelection.FirstOrDefault() as Todo));
    }
}
