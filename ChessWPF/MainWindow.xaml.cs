﻿using ChessWPF.UserControls;
using System.Windows;
using System.Windows.Input;

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
			contentControl.Content = new MenuUserControl();
		}

		private void FriendGameButton_Click(object sender, RoutedEventArgs e)
		{
            contentControl.Content = new GameUserControl(MainMenuButton);
			FriendGameButton.Visibility = Visibility.Hidden;
			ComputerGameButton.Visibility = Visibility.Hidden;
			StatisticButton.Visibility = Visibility.Hidden;
		}

		private void MainMenuButton_Click(object sender, RoutedEventArgs e)
		{
			contentControl.Content = new MenuUserControl();
			FriendGameButton.Visibility = Visibility.Visible;
			ComputerGameButton.Visibility = Visibility.Visible;
			StatisticButton.Visibility = Visibility.Visible;
			MainMenuButton.Visibility = Visibility.Hidden;
		}

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
			if (contentControl.Content is GameUserControl && e.Key == Key.Escape)
				(contentControl.Content as GameUserControl).TogglePauseMenu();
        }

        private void ComputerGameButton_Click(object sender, RoutedEventArgs e)
        {
            contentControl.Content = new GameUserControl(MainMenuButton, true);
            FriendGameButton.Visibility = Visibility.Hidden;
            ComputerGameButton.Visibility = Visibility.Hidden;
			StatisticButton.Visibility = Visibility.Hidden;
		}

		private void StatisticButton_Click(object sender, RoutedEventArgs e)
		{
			contentControl.Content = new StatisticUserControl(MainMenuButton);
			FriendGameButton.Visibility = Visibility.Hidden;
			ComputerGameButton.Visibility = Visibility.Hidden;
			StatisticButton.Visibility = Visibility.Hidden;
		}
	}
}