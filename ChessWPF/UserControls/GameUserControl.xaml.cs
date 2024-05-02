using ChessManagementClasses;
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
        public ChessTimer TimerWhite { get; protected set;}
        public ChessTimer TimerBlack { get; protected set; }
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
            mediaPlayer.Open(new Uri("../../../Assets/SFX/MoveSound.mp3", UriKind.Relative));
            mediaPlayer.Play();
            mediaPlayer.Stop();

            TimerWhite = new ChessTimer();
            TimerWhite.SetInterval(new TimeSpan(0, 0, 1));
            TimerWhite.SetTick(TimerTick);

            TimerBlack = new ChessTimer();
            TimerBlack.SetInterval(new TimeSpan(0, 0, 1));
            TimerBlack.SetTick(TimerTick);

            TimerWhiteTextBlock.Text = TimerWhite.Time;
            TimerBlackTextBlock.Text = TimerBlack.Time;

            OverlayMenu.Content = new TimerUserControl(this);
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

                            // Open promotion menu if the move is a promotion
                            PromotionMenuUserControl promotionMenu = new PromotionMenuUserControl(this, move as PromotionMove);
                            OverlayMenu.Content = promotionMenu;
                        }
                        else
                        {
                            // Handle move if it is not a Promotion move
                            ManageMove(move);
                            MoveToList(move);

                            if (IsPawnDoubleMove(move))
                                board.EnPassantPosition = new Position((move.StartPosition.Rank + move.EndPosition.Rank) / 2, 
                                    move.StartPosition.File);
                        }

                        // Play sound
                        mediaPlayer.Open(new Uri("../../../Assets/SFX/MoveSound.mp3", UriKind.Relative));
                        mediaPlayer.Play();

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
                            OverlayMenu.Content = gameEndMenu;
                            
                            TimerWhite.Stop();
                            TimerBlack.Stop();
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
            BoardGrid.Children.Clear();
            DrawPieces();
            StatusTextBlock.Text = board.CurrentPlayer == PieceColor.White ? "Хід білих" : "Хід чорних";
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

            if (IsTimerEnabled)
                TimerReset();
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

            if (move is RegularMove && (move as RegularMove).IsCapture || move is EnPassantMove)
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

        protected bool IsPawnDoubleMove(MoveBase move) =>
            board.GetPiece(move.EndPosition) is Pawn && Math.Abs(move.StartPosition.Rank - move.EndPosition.Rank) == 2;

        public void TogglePauseMenu()
        {
            if (OverlayMenu.Content is PauseUserControl)
            {
                OverlayMenu.Content = null;

                if (board.CurrentPlayer == PieceColor.White)
                    TimerWhite.Start();
                else
                    TimerBlack.Start();
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
                    StatusTextBlock.Text = "Час білих вичерпано";
                    GameEndUserControl gameEndMenu = new GameEndUserControl(this);
                    OverlayMenu.Content = gameEndMenu;
                }
            }
            else
            {
                TimerBlack.HandleTimer();
                TimerBlackTextBlock.Text = TimerBlack.Time;

                if (TimerBlack.Time == "00:00")
                {
                    board.GameOver = new GameOver(PieceColor.White, PossibleEndings.TimerIsOver);
                    StatusTextBlock.Text = "Час чорних вичерпано";
                    GameEndUserControl gameEndMenu = new GameEndUserControl(this);
                    OverlayMenu.Content = gameEndMenu;
                }
            }
        }
    }
}
