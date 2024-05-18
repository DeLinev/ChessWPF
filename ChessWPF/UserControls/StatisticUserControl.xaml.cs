using ChessManagementClasses;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace ChessWPF.UserControls
{
	/// <summary>
	/// Interaction logic for StatisticUserControl.xaml
	/// </summary>
	public partial class StatisticUserControl : UserControl
	{
		protected Button mainMenuButton;

		public StatisticUserControl(Button btn)
		{
			InitializeComponent();

			mainMenuButton = btn;

			string savesFolderPath = "../../../Saves/";
			string friendStatFileName = "friendStat.txt";
			string computerStatFileName = "computerStat.txt";

			string friendStatFilePath = Path.Combine(savesFolderPath, friendStatFileName);
			string computerStatFilePath = Path.Combine(savesFolderPath, computerStatFileName);

			if (Directory.Exists(savesFolderPath))
			{
				if (File.Exists(friendStatFilePath))
				{
					using (StreamReader sr = File.OpenText(friendStatFilePath))
					{
						string line = sr.ReadLine();
						string[] parts = line.Split(' ');

						FrWhite.Text = parts[0];
						FrBlack.Text = parts[1];
						FrDraw.Text = parts[2];
						FrMate.Text = parts[3];
						FrStalemate.Text = parts[4];
						FrTimer.Text = parts[5];
						FrInsuffMat.Text = parts[6];
						FrFiftyMovesRule.Text = parts[7];
					}
				}
				else
				{
					using (StreamWriter sw = File.CreateText(friendStatFilePath))
					{
						FrWhite.Text = "0";
						FrBlack.Text = "0";
						FrDraw.Text = "0";
						FrMate.Text = "0";
						FrStalemate.Text = "0";
						FrTimer.Text = "0";
						FrInsuffMat.Text = "0";
						FrFiftyMovesRule.Text = "0";

						sw.WriteLine("0 0 0 0 0 0 0 0");
					}
				}

				if (File.Exists(computerStatFilePath))
				{
					using (StreamReader sr = File.OpenText(computerStatFilePath))
					{
						string line = sr.ReadLine();
						string[] parts = line.Split(' ');

						CmWhite.Text = parts[0];
						CmBlack.Text = parts[1];
						CmDraw.Text = parts[2];
						CmMate.Text = parts[3];
						CmStalemate.Text = parts[4];
						CmTimer.Text = parts[5];
						CmInsuffMat.Text = parts[6];
						CmFiftyMovesRule.Text = parts[7];
					}
				}
				else
				{
					using (StreamWriter sw = File.CreateText(computerStatFilePath))
					{
						CmWhite.Text = "0";
						CmBlack.Text = "0";
						CmDraw.Text = "0";
						CmMate.Text = "0";
						CmStalemate.Text = "0";
						CmTimer.Text = "0";
						CmInsuffMat.Text = "0";
						CmFiftyMovesRule.Text = "0";

						sw.WriteLine("0 0 0 0 0 0 0 0");
					}
				}
			}
			else
			{
				Directory.CreateDirectory(savesFolderPath);

				using (StreamWriter sw = File.CreateText(friendStatFilePath))
				{
					FrWhite.Text = "0";
					FrBlack.Text = "0";
					FrDraw.Text = "0";
					FrMate.Text = "0";
					FrStalemate.Text = "0";
					FrTimer.Text = "0";
					FrInsuffMat.Text = "0";
					FrFiftyMovesRule.Text = "0";

					sw.WriteLine("0 0 0 0 0 0 0 0");
				}

				using (StreamWriter sw = File.CreateText(computerStatFilePath))
				{
					CmWhite.Text = "0";
					CmBlack.Text = "0";
					CmDraw.Text = "0";
					CmMate.Text = "0";
					CmStalemate.Text = "0";
					CmTimer.Text = "0";
					CmInsuffMat.Text = "0";
					CmFiftyMovesRule.Text = "0";

					sw.WriteLine("0 0 0 0 0 0 0 0");
				}
			}
		}

		private void ToMenuButton_Click(object sender, RoutedEventArgs e)
		{
			mainMenuButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
		}
	}
}
