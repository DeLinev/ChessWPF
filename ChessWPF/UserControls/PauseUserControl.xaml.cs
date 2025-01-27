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
			string savesFolderPath = "../../../Saves/";
            string gameStateFileName;
			string movesFileName;

			if (gameUserControl.IsComputerEnabled)
			{
				gameStateFileName = "computerSave.txt";
				movesFileName = "computerMoves.txt";
			}
			else
			{
				gameStateFileName = "friendSave.txt";
				movesFileName = "friendMoves.txt";
			}

            string gameStateFilePath = Path.Combine(savesFolderPath, gameStateFileName);
			string movesFilePath = Path.Combine(savesFolderPath, movesFileName);

			SaveGame(gameStateFilePath, movesFilePath, savesFolderPath);

            gameUserControl.ToMainMenu();
        }

		private void SaveGame(string gameStateFilePath, string movesFilePath, string savesFolderPath)
		{
			if(!Directory.Exists(savesFolderPath))
                Directory.CreateDirectory(savesFolderPath);

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
