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
            gameUserControl.ToMainMenu();
        }
    }
}
