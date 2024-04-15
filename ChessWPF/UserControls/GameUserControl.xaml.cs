using ChessManagementClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
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
		protected int movesCount = 1;
		protected Button mainMenuButton;
		protected Board board;
		protected Shape[,] possibleMovesOverlay;
		protected Position chosenPos;
		protected MediaPlayer mediaPlayer;
		protected List<MoveBase> possibleMoves;

		public Board Board { get => board; }

		public GameUserControl(Button btn)
		{
			InitializeComponent();
			StatusTextBlock.Text = "Хід білих";
			board = new Board();
			possibleMovesOverlay = new Shape[8, 8];
			possibleMoves = new List<MoveBase>();
			board.SetBoard();
			DrawPieces();
			mainMenuButton = btn;
			mediaPlayer = new MediaPlayer();
		}

		protected void DrawPieces()
		{
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					Image img = new();
					img.Source = GetImageSource(board.GetPiece(new Position(i, j)));
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
				foreach (var move in possibleMoves)
				{
					possibleMovesOverlay[move.EndPosition.currentRank, move.EndPosition.currentFile].Fill = Brushes.Transparent;

					if (move.EndPosition.currentRank == rank && move.EndPosition.currentFile == file)
					{
						// Make the move
						board.Move(move);
						BoardGrid.Children.Clear();
						DrawPieces();
						StatusTextBlock.Text = board.CurrentPlayer == PieceColor.White ? "Хід білих" : "Хід чорних";

						// Add move to the MovesList
						MoveToList(move);

						// Play sound
						mediaPlayer.Open(new Uri("../../../Assets/SFX/MoveSound.mp3", UriKind.Relative));
						mediaPlayer.Play();

						// Check if the game is over
						if (board.GameOver != null)
						{
							string winner = board.GameOver.Winner == PieceColor.White ? "Білі" : "Чорні";
							string ending = board.GameOver.Ending == PossibleEndings.CheckMate ? "Мат" : "Пат";

							if (board.GameOver.Winner == null)
								StatusTextBlock.Text = "Нічия через " + ending;
							else
								StatusTextBlock.Text = winner + " перемогли через " + ending;

							GameEndUserControl gameEndMenu = new GameEndUserControl(this);
							GameEndMenu.Content = gameEndMenu;
							mainMenuButton.Visibility = Visibility.Visible;
						}

						break;
					}
				}

				possibleMovesOverlay[chosenPos.currentRank, chosenPos.currentFile].Fill = Brushes.Transparent;
				chosenPos = null;

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

		public void ResetGame()
		{
			board = new Board();
			BoardGrid.Children.Clear();
			DrawPieces();
			StatusTextBlock.Text = "Хід білих";
			GameEndMenu.Content = null;
			mainMenuButton.Visibility = Visibility.Hidden;
			movesCount = 1;
			MovesList.Items.Clear();
		}

		protected void MoveToList(MoveBase move)
		{
			PieceBase piece = board.GetPiece(move.EndPosition);
			StackPanel stackPanel = new StackPanel();
			TextBlock moveNumber = new TextBlock();
			Image img = new Image();
			TextBlock moveText = new TextBlock();

			stackPanel.Orientation = Orientation.Horizontal;

			moveNumber.VerticalAlignment = VerticalAlignment.Bottom;
			moveNumber.FontFamily = new FontFamily("/Fonts/Roboto-Light.ttf #Roboto");
			moveNumber.FontSize = 25;

			if (piece.Color == PieceColor.White)
			{
				moveNumber.Text = movesCount + ".";
				movesCount++;
				stackPanel.Children.Add(moveNumber);

				if (piece is Pawn)
					moveNumber.Margin = new Thickness(0, 0, 5, 0);
			}

			if (!(piece is Pawn))
			{
				img.Width = 40;
				img.Source = GetImageSource(piece);
			}

			moveText.VerticalAlignment = VerticalAlignment.Center;

			if (move is RegularMove && (move as RegularMove).IsCapture)
			{
				if (piece is Pawn)
				{
					moveText.Text = move.StartPosition.GetFileLetter();
					moveNumber.Margin = new Thickness(0, 0, 5, 0);
				}

				moveText.Text += "x";
			}

			moveText.Text += move.EndPosition.ToString();

			if (board.GameOver != null)
				moveText.Text += "#";
			else if (piece.IsThreatToKing(board, move.EndPosition))
				moveText.Text += "+";

			stackPanel.Children.Add(img);
			stackPanel.Children.Add(moveText);

			MovesList.Items.Add(stackPanel);
			MovesList.ScrollIntoView(stackPanel);
		}

		protected BitmapImage GetImageSource(PieceBase piece)
		{
			BitmapImage bitmapImage = new BitmapImage();

			switch (piece)
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
					bitmapImage = null;
					break;
			}

			return bitmapImage;
		}
	}
}
