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
		protected Board board;
		protected Shape[,] possibleMovesOverlay;
		protected Position chosenPos;
		protected List<MoveBase> possibleMoves;

		public GameUserControl()
		{
			InitializeComponent();
			StatusTextBlock.Text = "White to move";
			board = new Board();
			possibleMovesOverlay = new Shape[8, 8];
			possibleMoves = new List<MoveBase>();
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

					Ellipse ellipse = new();
					possibleMovesOverlay[i, j] = ellipse;
					//OverlayGrid.Children.Add(ellipse);
				}
			}
		}

		private void GameGrid_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Grid grid = (Grid)sender;

			Point mousePosition = e.GetPosition(grid);

			if (mousePosition.X > BoardGrid.ActualWidth || mousePosition.Y < StatusTextBlock.ActualHeight)
				return;

			// Calculate the cell that was clicked
			double cellHeight = BoardGrid.ActualHeight / 8;
			double cellWidth = BoardGrid.ActualWidth / 8;
			double actualYOfClick = mousePosition.Y - StatusTextBlock.ActualHeight;
			double actualXOfClick = mousePosition.X;
			int rank = (int)(actualYOfClick / cellHeight);
			int file = (int)(actualXOfClick / cellWidth);

			if (possibleMoves.Count == 0)
				possibleMoves = board.GetPossibleMoves(new Position(rank, file));

			if (chosenPos != null)
			{
				possibleMovesOverlay[chosenPos.currentRank, chosenPos.currentFile].Fill = Brushes.Transparent;
				chosenPos = null;

				foreach (var move in possibleMoves)
				{
					possibleMovesOverlay[move.EndPosition.currentRank, move.EndPosition.currentFile].Fill = Brushes.Transparent;

					if (move.EndPosition.currentRank == rank && move.EndPosition.currentFile == file)
					{
						board.Move(move);
						BoardGrid.Children.Clear();
						DrawPieces();
						StatusTextBlock.Text = board.CurrentPlayer == PieceColor.White ? "White to move" : "Black to move";

						if (board.GameOver != null)
						{
							string winner = board.GameOver.Winner == PieceColor.White ? "White" : "Black";
							string ending = board.GameOver.Ending == PossibleEndings.CheckMate ? "Checkmate" : "Stalemate";

							if (board.GameOver.Winner == null)
								StatusTextBlock.Text = "Draw by " + ending;
							else
								StatusTextBlock.Text = winner + " wins by " + ending;
						}

						break;
					}
				}

				OverlayGrid.Children.Clear();
				possibleMoves.Clear();

            }
			else
			{
				if (possibleMoves.Count > 0)
				{
					chosenPos = new Position(rank, file);
					
					Color col = Color.FromRgb(153, 145, 236);

					foreach (var move in possibleMoves)
					{
						possibleMovesOverlay[move.EndPosition.currentRank, move.EndPosition.currentFile].Fill = new SolidColorBrush(col);
						possibleMovesOverlay[move.EndPosition.currentRank, move.EndPosition.currentFile].Height = 20;
						possibleMovesOverlay[move.EndPosition.currentRank, move.EndPosition.currentFile].Width = 20;
					}

					possibleMovesOverlay[rank, file] = new Rectangle();
					possibleMovesOverlay[rank, file].Fill = new SolidColorBrush(col);

					AddToOverlayGrid();
				}
			}
		}

		protected void AddToOverlayGrid()
		{
			OverlayGrid.Children.Clear();

			for (int i = 0; i < 8; i++)
				for (int j = 0; j < 8; j++)
					OverlayGrid.Children.Add(possibleMovesOverlay[i, j]);
		}
	}
}
