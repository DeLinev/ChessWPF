using ChessManagementClasses;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ChessWPF.UserControls
{
    /// <summary>
    /// Interaction logic for PromotionMenuUserControl.xaml
    /// </summary>
    public partial class PromotionMenuUserControl : UserControl
    {
        protected GameUserControl gameUserControl;
        protected PromotionMove move;

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
		}
	}
}
