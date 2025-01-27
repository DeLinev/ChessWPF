using ChessManagementClasses;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ChessWPF.UserControls
{
	/// <summary>
	/// Interaction logic for PauseUserControl.xaml
	/// </summary>
	public partial class PauseUserControl : UserControl
    {
        protected GameUserControl gameUserControl;

        public PauseUserControl(GameUserControl usrCntrl)
        {
            InitializeComponent();
            gameUserControl = usrCntrl;
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            gameUserControl.TogglePauseMenu();
        }

        private void NewGameButton_Click(object sender, RoutedEventArgs e)
        {
            gameUserControl.ResetGame();
        }

        private void MainMenuButton_Click(object sender, RoutedEventArgs e)
        {
            string gameStateFilePath = Board.GetGameStateFilePath(gameUserControl.IsComputerEnabled);
			string movesFilePath = Board.GetGameMovesFilePath(gameUserControl.IsComputerEnabled);

            SaveGame(gameStateFilePath, movesFilePath);

            gameUserControl.ToMainMenu();
        }

		private void SaveGame(string gameStateFilePath, string movesFilePath)
		{
			if(!Directory.Exists(Board.SaveFilePath))
                Directory.CreateDirectory(Board.SaveFilePath);

            File.Delete(gameStateFilePath);
            File.Delete(movesFilePath);

            using (StreamWriter sw = File.CreateText(gameStateFilePath))
            {
                string gameState = gameUserControl.Board.GetFenString();

                if (gameUserControl.IsTimerEnabled)
                    gameState += " " + gameUserControl.TimerWhite.Time + " " + gameUserControl.TimerBlack.Time
                        + " " + gameUserControl.TimerWhite.InitialTime;
                else
                    gameState += " - - -";

                sw.WriteLine(gameState);
            }

            using (StreamWriter sw = File.CreateText(movesFilePath))
            {
                sw.WriteLine(gameUserControl.Moves);
            }
        }
    }
}
