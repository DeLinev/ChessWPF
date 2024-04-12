using ChessWPF.UserControls;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessWPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		public MainWindow()
		{
			InitializeComponent();
			this.contentControl.Content = new MenuUserControl();
		}

		private void FriendGameButton_Click(object sender, RoutedEventArgs e)
		{
			this.contentControl.Content = new GameUserControl(MainMenuButton);
			FriendGameButton.Visibility = Visibility.Hidden;
			ComputerGameButton.Visibility = Visibility.Hidden;
		}

		private void MainMenuButton_Click(object sender, RoutedEventArgs e)
		{
			this.contentControl.Content = new MenuUserControl();
			FriendGameButton.Visibility = Visibility.Visible;
			ComputerGameButton.Visibility = Visibility.Visible;
			MainMenuButton.Visibility = Visibility.Hidden;
		}
	}
}