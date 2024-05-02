using ChessManagementClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChessWPF.UserControls
{
	/// <summary>
	/// Interaction logic for GameEndUserControl.xaml
	/// </summary>
	public partial class GameEndUserControl : UserControl
	{
		protected GameUserControl userControl;
		public GameEndUserControl(GameUserControl usrCntrl)
		{
			InitializeComponent();
			userControl = usrCntrl;
			WinnerTextBlock.Text = userControl.Board.GameOver.Winner == null ? "Нічия" : 
				userControl.Board.GameOver.Winner == PieceColor.White ? "Білі перемогли!" : "Чорні перемогли!";
			switch (userControl.Board.GameOver.Ending)
			{
                case PossibleEndings.CheckMate:
                    ReasonTextBlock.Text = "мат";
                    break;
                case PossibleEndings.StaleMate:
                    ReasonTextBlock.Text = "пат";
                    break;
                case PossibleEndings.TimerIsOver:
                    ReasonTextBlock.Text = "час вичерпано";
                    break;
            }
		}

		private void NewGameButton_Click(object sender, RoutedEventArgs e)
		{
			userControl.ResetGame();
		}

        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
			userControl.ToMainMenu();
        }
    }
}
