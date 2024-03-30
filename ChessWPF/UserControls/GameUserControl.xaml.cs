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
	/// Interaction logic for GameUserControl.xaml
	/// </summary>
	public partial class GameUserControl : UserControl
	{
		//protected Image[,] ChessPieces;
		protected Board board = new Board();

		public GameUserControl()
		{
			InitializeComponent();
			MoveSideTextBlock.Text = "White to move";
			//ChessPieces = new Image[8, 8];
			board.SetBoard();
			DrawPieces();
		}

		protected void DrawPieces()
		{
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					Image img = new();
					BitmapImage bitmapImage = new();

					switch (board.GetPiece(new Position(i, j)))
					{
						case Pawn p:
							if (p.Color == PieceColor.White)
							{
								bitmapImage.BeginInit();
								bitmapImage.UriSource = new Uri("/Assets/WhitePawn.png", UriKind.Relative);
								bitmapImage.EndInit();
							}
							else
							{
								bitmapImage.BeginInit();
								bitmapImage.UriSource = new Uri("/Assets/BlackPawn.png", UriKind.Relative);
								bitmapImage.EndInit();
							}
							break;
						case Rook r:
							if (r.Color == PieceColor.White)
							{
								bitmapImage.BeginInit();
								bitmapImage.UriSource = new Uri("/Assets/WhiteRook.png", UriKind.Relative);
								bitmapImage.EndInit();
							}
							else
							{
								bitmapImage.BeginInit();
								bitmapImage.UriSource = new Uri("/Assets/BlackRook.png", UriKind.Relative);
								bitmapImage.EndInit();
							}
							break;
						case Knight k:
							if (k.Color == PieceColor.White)
							{
								bitmapImage.BeginInit();
								bitmapImage.UriSource = new Uri("/Assets/WhiteKnight.png", UriKind.Relative);
								bitmapImage.EndInit();
							}
							else
							{
								bitmapImage.BeginInit();
								bitmapImage.UriSource = new Uri("/Assets/BlackKnight.png", UriKind.Relative);
								bitmapImage.EndInit();
							}
							break;
						case Bishop b:
							if (b.Color == PieceColor.White)
							{
								bitmapImage.BeginInit();
								bitmapImage.UriSource = new Uri("/Assets/WhiteBishop.png", UriKind.Relative);
								bitmapImage.EndInit();
							}
							else
							{
								bitmapImage.BeginInit();
								bitmapImage.UriSource = new Uri("/Assets/BlackBishop.png", UriKind.Relative);
								bitmapImage.EndInit();
							}
							break;
						case Queen q:
							if (q.Color == PieceColor.White)
							{
								bitmapImage.BeginInit();
								bitmapImage.UriSource = new Uri("/Assets/WhiteQueen.png", UriKind.Relative);
								bitmapImage.EndInit();
							}
							else
							{
								bitmapImage.BeginInit();
								bitmapImage.UriSource = new Uri("/Assets/BlackQueen.png", UriKind.Relative);
								bitmapImage.EndInit();
							}
							break;
						case King k:
							if (k.Color == PieceColor.White)
							{
								bitmapImage.BeginInit();
								bitmapImage.UriSource = new Uri("/Assets/WhiteKing.png", UriKind.Relative);
								bitmapImage.EndInit();
							}
							else
							{
								bitmapImage.BeginInit();
								bitmapImage.UriSource = new Uri("/Assets/BlackKing.png", UriKind.Relative);
								bitmapImage.EndInit();
							}
							break;
						default:
							img.Source = null;
							break;
					}

					img.Source = bitmapImage;
					img.Width = 60;
					BoardGrid.Children.Add(img);
				}
			}
		}
	}
}
