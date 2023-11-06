using Avalonia;
using Avalonia.Controls;

namespace FootballLeaguesStandingsTableProject.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        this.WindowState = WindowState.Maximized;
        this.WindowStartupLocation = WindowStartupLocation.CenterOwner;
        this.CanResize = true;
	}


}
