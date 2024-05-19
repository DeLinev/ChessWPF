using ChessManagementClasses;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;

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

		public bool IsTimerEnabled { get; set; } = true;
		public bool IsComputerEnabled { get; }
		public ChessTimer TimerWhite { get; protected set; }
		public ChessTimer TimerBlack { get; protected set; }
		public Board Board { get => board; }
		public string Moves { get; protected set; }

		public GameUserControl(Button btn, bool isComputerEnabled = false)
		{
			InitializeComponent();
			StatusTextBlock.Text = "Хід білих";
			board = new Board();
			possibleMovesOverlay = new Shape[8, 8];
			possibleMoves = new List<MoveBase>();
			IsComputerEnabled = isComputerEnabled;

			TimerWhite = new ChessTimer();
			TimerWhite.SetInterval(new TimeSpan(0, 0, 1));
			TimerWhite.SetTick(TimerTick);

			TimerBlack = new ChessTimer();
			TimerBlack.SetInterval(new TimeSpan(0, 0, 1));
			TimerBlack.SetTick(TimerTick);

			bool isLoaded = SetBoardFromSaves();

			if (!isLoaded)
				board.SetBoard();

			DrawPieces();
			mainMenuButton = btn;

			mediaPlayer = new MediaPlayer();
			mediaPlayer.Open(new Uri("../../../Assets/SFX/MoveSound.mp3", UriKind.Relative));
			mediaPlayer.Play();
			mediaPlayer.Stop();

			if (!IsComputerEnabled)
			{
				if (isLoaded)
				{
					if (IsTimerEnabled)
					{
						switch (board.CurrentPlayer)
						{
							case PieceColor.White:
								TimerWhite.Start();
								break;
							case PieceColor.Black:
								TimerBlack.Start();
								break;
						}

						TimerWhiteTextBlock.Text = TimerWhite.Time;
						TimerBlackTextBlock.Text = TimerBlack.Time;
					}
				}
				else
					OverlayMenu.Content = new TimerUserControl(this);
			}
			else
				IsTimerEnabled = false;
		}

		protected bool SetBoardFromSaves()
		{
			string savesFolderPath = "../../../Saves/";
			string gameStateFileName;
			string movesFileName;

			if (IsComputerEnabled)
			{
				gameStateFileName = "computerSave.txt";
				movesFileName = "computerMoves.txt";
			}
			else
			{
				gameStateFileName = "friendSave.txt";
				movesFileName = "friendMoves.txt";
			}

			string gameStateFilePath = System.IO.Path.Combine(savesFolderPath, gameStateFileName);
			string movesFilePath = System.IO.Path.Combine(savesFolderPath, movesFileName);

			if (Directory.Exists(savesFolderPath))
			{
				if (File.Exists(gameStateFilePath))
				{
					using (StreamReader sr = File.OpenText(gameStateFilePath))
					{
						string line = sr.ReadLine();
						string[] parts = line.Split(' ');

						string fenStr = parts[0] + ' ' + parts[1] + ' ' + parts[2] + ' ' + parts[3];

						board.SetBoard(fenStr);

						if (parts[4] != "-" && parts[5] != "-")
						{
							TimerWhite.SetTime(parts[4], parts[6]);
							TimerBlack.SetTime(parts[5], parts[6]);
						}
						else
							IsTimerEnabled = false;
					}
				}
				else
					return false;

				if (File.Exists(movesFilePath))
				{
					using (StreamReader sr = File.OpenText(movesFilePath))
					{
						Moves = sr.ReadLine();
						string[] moves = Moves.Split(' ');

						for (int i = 0; i < moves.Length; i++)
						{
							if (moves[i] == "")
								continue;

							MoveToList(moves[i], i);
						}
					}
				}
				else
					return false;

				return true;
			}
			else
				return false;
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
				}
			}
		}

		private void GameGrid_MouseDown(object sender, MouseButtonEventArgs e)
		{
			Grid grid = (Grid)sender;

			Point mousePosition = e.GetPosition(grid);

			if (mousePosition.X > BoardGrid.ActualWidth
				|| mousePosition.Y < StatusTextBlock.ActualHeight
				|| OverlayMenu.Content != null)
				return;

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
					possibleMovesOverlay[move.EndPosition.Rank, move.EndPosition.File].Fill = Brushes.Transparent;

					if (move.EndPosition.Rank == rank && move.EndPosition.File == file)
					{
						if (move is PromotionMove)
						{
							RegularMove regMove = new RegularMove(move.StartPosition, move.EndPosition);
							regMove.MakeMove(board);
							BoardGrid.Children.Clear();
							DrawPieces();
							regMove.ReverseMove(board);

							PromotionMenuUserControl promotionMenu = new PromotionMenuUserControl(this, move as PromotionMove);
							OverlayMenu.Content = promotionMenu;
						}
						else
						{
							ManageMove(move);
						}

						if (IsTimerEnabled)
						{
							if (board.CurrentPlayer == PieceColor.Black)
							{
								TimerBlack.Start();
								TimerWhite.Stop();
							}
							else
							{
								TimerWhite.Start();
								TimerBlack.Stop();
							}
						}

						if (board.GameOver != null)
							EndGame(board.GameOver);
						else if (IsComputerEnabled)
						{
							DispatcherTimer timer = new();
							timer.Interval = new TimeSpan(0, 0, 0, 0, 1);
							timer.Tick += (object? sender, EventArgs e) =>
							{
								ComputerMove computerMove = new ComputerMove();
								ManageMove(computerMove);

								if (board.GameOver != null)
									EndGame(board.GameOver);

								timer.Stop();
							};
							timer.Start();
						}

						break;
					}
				}

				ClearOverlayGrid();
			}
			else
			{
				if (possibleMoves.Count > 0)
				{
					chosenPos = new Position(rank, file);

					Color col = Color.FromRgb(153, 145, 236);

					foreach (var move in possibleMoves)
					{
						possibleMovesOverlay[move.EndPosition.Rank, move.EndPosition.File].Fill = new SolidColorBrush(col);
						possibleMovesOverlay[move.EndPosition.Rank, move.EndPosition.File].Height = 20;
						possibleMovesOverlay[move.EndPosition.Rank, move.EndPosition.File].Width = 20;
					}

					possibleMovesOverlay[rank, file] = new Rectangle();
					possibleMovesOverlay[rank, file].Fill = new SolidColorBrush(col);

					AddToOverlayGrid();
				}
			}
		}

		public void ManageMove(MoveBase move)
		{
			board.Move(move);

			if (IsPawnDoubleMove(move))
				board.EnPassantPosition = new Position((move.StartPosition.Rank + move.EndPosition.Rank) / 2,
					move.StartPosition.File);

			MoveToList(move);
			mediaPlayer.Open(new Uri("../../../Assets/SFX/MoveSound.mp3", UriKind.Relative));
			mediaPlayer.Play();
			BoardGrid.Children.Clear();
			DrawPieces();
			StatusTextBlock.Text = board.CurrentPlayer == PieceColor.White ? "Хід білих" : "Хід чорних";
		}

		protected void EndGame(GameOver info)
		{
			StatusTextBlock.Text = "Кінець гри!";

			GameEndUserControl gameEndMenu = new GameEndUserControl(this);
			OverlayMenu.Content = gameEndMenu;

			TimerWhite.Stop();
			TimerBlack.Stop();

			string savesFolderPath = "../../../Saves/";
			string gameStateFileName;
			string movesFileName;

			if (IsComputerEnabled)
			{
				gameStateFileName = "computerSave.txt";
				movesFileName = "computerMoves.txt";
			}
			else
			{
				gameStateFileName = "friendSave.txt";
				movesFileName = "friendMoves.txt";
			}

			string gameStateFilePath = System.IO.Path.Combine(savesFolderPath, gameStateFileName);
			string movesFilePath = System.IO.Path.Combine(savesFolderPath, movesFileName);

			if (Directory.Exists(savesFolderPath))
			{
				if (File.Exists(gameStateFilePath))
				{
					File.Delete(gameStateFilePath);
				}

				if (File.Exists(movesFilePath))
				{
					File.Delete(movesFilePath);
				}
			}

			HandleStat(info);
		}

		protected void HandleStat(GameOver info)
		{
			Dictionary<PossibleEndings, int> reasonStatIndexes = new()
			{
				{ PossibleEndings.CheckMate, 3 },
				{ PossibleEndings.StaleMate, 4 },
				{ PossibleEndings.TimerIsOver, 5 },
				{ PossibleEndings.InsuffMaterial, 6 },
				{ PossibleEndings.FiftyMoveRule, 7 },
			};

			Dictionary<PieceColor?, int> winnerStatIndexes = new()
			{
				{ PieceColor.White, 0 },
				{ PieceColor.Black, 1 },
			};

			string savesFolderPath = "../../../Saves/";
			string statFileName;

			if (IsComputerEnabled)
				statFileName = "computerStat.txt";
			else
				statFileName = "friendStat.txt";

			string gameStatFilePath = System.IO.Path.Combine(savesFolderPath, statFileName);

			if (Directory.Exists(savesFolderPath))
			{
				if (File.Exists(gameStatFilePath))
				{
					int[] stats = new int[8];

					using (StreamReader sr = File.OpenText(gameStatFilePath))
					{
						string line = sr.ReadLine();
						string[] parts = line.Split(' ');

						for (int i = 0; i < 8; i++)
							stats[i] = int.Parse(parts[i]);

						if (info.Winner == null)
							stats[2]++;
						else
							stats[winnerStatIndexes[info.Winner]]++;

						stats[reasonStatIndexes[info.Ending]]++;
					}

					File.Delete(gameStatFilePath);

					using (StreamWriter sw = File.CreateText(gameStatFilePath))
					{
						sw.WriteLine(string.Join(" ", stats));
					}
				}
				else
				{
					using (StreamWriter sw = File.CreateText(gameStatFilePath))
					{
						int[] stats = { 0, 0, 0, 0, 0, 0, 0, 0 };

						if (info.Winner == null)
							stats[2]++;
						else
							stats[winnerStatIndexes[info.Winner]]++;

						stats[reasonStatIndexes[info.Ending]]++;

						sw.WriteLine(string.Join(" ", stats));
					}
				}
			}
			else
			{
				Directory.CreateDirectory(savesFolderPath);

				using (StreamWriter sw = File.CreateText(gameStatFilePath))
				{
					int[] stats = { 0, 0, 0, 0, 0, 0, 0, 0 };

					if (info.Winner == null)
						stats[2]++;
					else
						stats[winnerStatIndexes[info.Winner]]++;

					stats[reasonStatIndexes[info.Ending]]++;

					sw.WriteLine(string.Join(" ", stats));
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

		protected void ClearOverlayGrid()
		{
			if (chosenPos != null)
			{
				possibleMovesOverlay[chosenPos.Rank, chosenPos.File].Fill = Brushes.Transparent;
				chosenPos = null;
				OverlayGrid.Children.Clear();
				possibleMoves.Clear();
			}
		}

		public void ResetGame()
		{
			ClearOverlayGrid();
			board = new Board();
			BoardGrid.Children.Clear();
			DrawPieces();
			StatusTextBlock.Text = "Хід білих";
			OverlayMenu.Content = null;
			mainMenuButton.Visibility = Visibility.Hidden;
			movesCount = 1;
			MovesList.Items.Clear();
			Moves = "";

			if (!IsComputerEnabled)
			{
				TimerWhiteTextBlock.Text = "--:--";
				TimerBlackTextBlock.Text = "--:--";
				OverlayMenu.Content = new TimerUserControl(this);
			}
		}

		protected void TimerReset()
		{
			TimerWhite.Reset();
			TimerBlack.Reset();
			TimerWhite.Start();
			TimerWhiteTextBlock.Text = TimerWhite.Time;
			TimerBlackTextBlock.Text = TimerBlack.Time;
		}

		public void ToMainMenu()
		{
			mainMenuButton.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
		}

		public void MoveToList(MoveBase move)
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
			}

			if (piece is Pawn || move is PromotionMove || move is CastlingMove)
				moveText.Margin = new Thickness(5, 0, 0, 0);
			else
			{
				img.Width = 40;
				img.Source = GetImageSource(piece);
			}

			moveText.VerticalAlignment = VerticalAlignment.Center;

			if (move.IsCapture)
			{
				if (piece is Pawn)
					moveText.Text = move.StartPosition.GetFileLetter();

				moveText.Text += "x";
			}

			moveText.Text += move.EndPosition.ToString();

			if (move is PromotionMove)
				moveText.Text += "=" + (move as PromotionMove).NewPiece;

			if (move is CastlingMove)
				moveText.Text = (move as CastlingMove).castlingDirection == PositionChanges.Right ? "0-0" : "0-0-0";

			if (board.GameOver != null && board.GameOver.Ending == PossibleEndings.CheckMate)
				moveText.Text += "#";
			else if (piece.IsThreatToKing(board, move.EndPosition))
				moveText.Text += "+";

			stackPanel.Children.Add(img);
			stackPanel.Children.Add(moveText);

			MovesList.Items.Add(stackPanel);
			MovesList.ScrollIntoView(stackPanel);

			if (!(move is PromotionMove || move is CastlingMove))
				Moves += piece.GetPieceChar() + moveText.Text + " ";
			else
				Moves += moveText.Text + " ";
		}

		public void MoveToList(string move, int moveNum)
		{
			bool isCasteling = false, isPromotion = false;
			PieceBase piece;

			if (move.Contains('='))
			{
				isPromotion = true;
				piece = moveNum % 2 != 0 ? new Queen(PieceColor.Black) : new Queen(PieceColor.White);
			}
			else if (move[0] == '0')
			{
				isCasteling = true;
				piece = moveNum % 2 != 0 ? new King(PieceColor.Black) : new King(PieceColor.White);
			}
			else
			{
				switch (move[0])
				{
					case 'p':
						piece = new Pawn(PieceColor.Black);
						break;
					case 'r':
						piece = new Rook(PieceColor.Black);
						break;
					case 'n':
						piece = new Knight(PieceColor.Black);
						break;
					case 'b':
						piece = new Bishop(PieceColor.Black);
						break;
					case 'q':
						piece = new Queen(PieceColor.Black);
						break;
					case 'k':
						piece = new King(PieceColor.Black);
						break;
					case 'P':
						piece = new Pawn(PieceColor.White);
						break;
					case 'R':
						piece = new Rook(PieceColor.White);
						break;
					case 'N':
						piece = new Knight(PieceColor.White);
						break;
					case 'B':
						piece = new Bishop(PieceColor.White);
						break;
					case 'Q':
						piece = new Queen(PieceColor.White);
						break;
					case 'K':
						piece = new King(PieceColor.White);
						break;
					default:
						throw new Exception("Invalid piece");
				}
			}

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
			}

			moveText.Text = move.Substring(1);

			if (piece is Pawn || isPromotion || isCasteling)
			{
				moveText.Margin = new Thickness(5, 0, 0, 0);

				if (isPromotion || isCasteling)
					moveText.Text = move;
			}
			else
			{
				img.Width = 40;
				img.Source = GetImageSource(piece);
			}

			moveText.VerticalAlignment = VerticalAlignment.Center;

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

		protected bool IsPawnDoubleMove(MoveBase move) =>
			board.GetPiece(move.EndPosition) is Pawn && Math.Abs(move.StartPosition.Rank - move.EndPosition.Rank) == 2;

		public void TogglePauseMenu()
		{
			if (OverlayMenu.Content is PauseUserControl)
			{
				OverlayMenu.Content = null;

				if (IsTimerEnabled)
				{
					if (board.CurrentPlayer == PieceColor.White)
						TimerWhite.Start();
					else
						TimerBlack.Start();
				}
			}
			else if (OverlayMenu.Content != null)
			{
				return;
			}
			else
			{
				PauseUserControl pauseMenu = new PauseUserControl(this);
				OverlayMenu.Content = pauseMenu;
				TimerWhite.Stop();
				TimerBlack.Stop();
			}
		}

		private void TimerTick(object? sender, EventArgs e)
		{
			if (board.CurrentPlayer == PieceColor.White)
			{
				TimerWhite.HandleTimer();
				TimerWhiteTextBlock.Text = TimerWhite.Time;

				if (TimerWhite.Time == "00:00")
				{
					board.GameOver = new GameOver(PieceColor.Black, PossibleEndings.TimerIsOver);

					EndGame(board.GameOver);
				}
			}
			else
			{
				TimerBlack.HandleTimer();
				TimerBlackTextBlock.Text = TimerBlack.Time;

				if (TimerBlack.Time == "00:00")
				{
					board.GameOver = new GameOver(PieceColor.White, PossibleEndings.TimerIsOver);

					EndGame(board.GameOver);
				}
			}
		}
	}
}
