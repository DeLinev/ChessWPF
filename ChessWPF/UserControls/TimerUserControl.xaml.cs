using System.Windows;
using System.Windows.Controls;

namespace ChessWPF.UserControls
{
	/// <summary>
	/// Interaction logic for TimerUserControl.xaml
	/// </summary>
	public partial class TimerUserControl : UserControl
    {
        protected GameUserControl gameUserControl;

        public TimerUserControl(GameUserControl usrCtrl)
        {
            InitializeComponent();
            gameUserControl = usrCtrl;

            for (int i = 0; i <= 60; i += 5)
            {
                if (i < 10)
                {
                    MinutesComboBox.Items.Add("0" + i);
                    SecondsComboBox.Items.Add("0" + i);
                }
                else
                {
                    MinutesComboBox.Items.Add(i);
                    SecondsComboBox.Items.Add(i);
                }
            }

            MinutesComboBox.SelectedIndex = 2;
            SecondsComboBox.SelectedIndex = 0;
        }

        private void ContinueButton_Click(object sender, RoutedEventArgs e)
        {
            gameUserControl.OverlayMenu.Content = null;
            gameUserControl.TimerWhite.SetTime(int.Parse(MinutesComboBox.Text), int.Parse(SecondsComboBox.Text));
            gameUserControl.TimerBlack.SetTime(int.Parse(MinutesComboBox.Text), int.Parse(SecondsComboBox.Text));
            gameUserControl.TimerWhite.Start();
            gameUserControl.TimerWhiteTextBlock.Text = gameUserControl.TimerWhite.Time;
            gameUserControl.TimerBlackTextBlock.Text = gameUserControl.TimerBlack.Time;
			gameUserControl.IsTimerEnabled = true;
		}

        private void ContinueAltButton_Click(object sender, RoutedEventArgs e)
        {
            gameUserControl.OverlayMenu.Content = null;
            gameUserControl.IsTimerEnabled = false;
            gameUserControl.TimerWhiteTextBlock.Text = "--:--";
            gameUserControl.TimerBlackTextBlock.Text = "--:--";
        }
    }
}
