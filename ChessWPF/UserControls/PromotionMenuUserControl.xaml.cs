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
    /// Interaction logic for PromotionMenuUserControl.xaml
    /// </summary>
    public partial class PromotionMenuUserControl : UserControl
    {
        GameUserControl gameUserControl;
        PromotionMove move;

        public PromotionMenuUserControl(GameUserControl usrCntrl, PromotionMove promMove)
        {
            InitializeComponent();

            gameUserControl = usrCntrl;
            move = promMove;
        }

		private void SelectStackPanel_MouseDown(object sender, MouseButtonEventArgs e)
		{
            StackPanel sp = (StackPanel)sender;

            Point mousePosition = e.GetPosition(sp);

			if (mousePosition.X > SelectStackPanel.ActualWidth || mousePosition.Y > SelectStackPanel.ActualHeight)
				return;

			double cellWidth = SelectStackPanel.ActualWidth / 4;
            int index = (int)(mousePosition.X / cellWidth);

            PieceBase[] pieces = [ new Queen(gameUserControl.Board.CurrentPlayer), new Rook(gameUserControl.Board.CurrentPlayer), 
                new Bishop(gameUserControl.Board.CurrentPlayer), new Knight(gameUserControl.Board.CurrentPlayer) ];

            MoveBase prMove = new PromotionMove(move.StartPosition, move.EndPosition, pieces[index].Type);

            gameUserControl.ManageMove(prMove);
			gameUserControl.OverlayMenu.Content = null;
            gameUserControl.MoveToList(prMove);
		}
	}
}
