namespace ChessManagementClasses
{
    public class ComputerMove : MoveBase
    {
        private const int maxDepth = 2;
        private const int centralBonus = 2;
        static private readonly Dictionary<ChessPieceType, int> pieceWeights = new Dictionary<ChessPieceType, int>()
        {
            { ChessPieceType.Pawn, 1 },
            { ChessPieceType.Knight, 3 },
            { ChessPieceType.Bishop, 3 },
            { ChessPieceType.Rook, 5 },
            { ChessPieceType.Queen, 9 },
            { ChessPieceType.King, 100 }
        };

        protected List<MoveBase> possibleMoves;
        protected MoveBase bestMove;

        public MoveBase BestMove
        {
            get => bestMove;
            set
            {
                bestMove = value;
                StartPosition = value.StartPosition;
                EndPosition = value.EndPosition;
            }
        }

        public ComputerMove()
        {
            possibleMoves = new List<MoveBase>();
        }

        public override void MakeMove(Board board)
        {
            possibleMoves = GetMoves(board);
            BestMove = FindBestMove(board);
            BestMove.MakeMove(board);
            CapturedPiece = BestMove.CapturedPiece;
        }

        public override void ReverseMove(Board board)
        {
            bestMove.ReverseMove(board);
        }

        private List<MoveBase> GetMoves(Board board)
        {
            List<MoveBase> moves = new List<MoveBase>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    PieceBase piece = board.GetPiece(new Position(i, j));

                    if (piece != null && piece.Color == board.CurrentPlayer)
                        moves.AddRange(board.GetPossibleMoves(new Position(i, j)));
                }
            }

            return moves;
        }

        private MoveBase FindBestMove(Board board)
        {
            MoveBase chosenMove = null;
            int bestScore = int.MinValue;
            int alpha = int.MinValue;
            int beta = int.MaxValue;

            foreach (MoveBase move in possibleMoves)
            {
                Board boardCopy = new Board(board);
                boardCopy.Move(move);
                int score = Minimax(boardCopy, maxDepth, alpha, beta, false);

                if (score > bestScore)
                {
                    bestScore = score;
                    chosenMove = move;
                }

                alpha = Math.Max(alpha, bestScore);

                if (beta <= alpha)
                    break;
            }

            return chosenMove;
        }

        private int Minimax(Board board, int depth, int alpha, int beta, bool isPlayer)
        {
            if (depth == 0 || board.GameOver != null)
                return Evaluate(board);

            List<MoveBase> moves = GetMoves(board);
            int maxScore = isPlayer 
                ? int.MinValue 
                : int.MaxValue;

            foreach (MoveBase move in moves)
            {
                Board boardCopy = new Board(board);
                boardCopy.Move(move);
                int score = Minimax(boardCopy, depth - 1, alpha, beta, !isPlayer);

                if (isPlayer)
                {
                    maxScore = Math.Max(maxScore, score);
                    alpha = Math.Max(alpha, maxScore);
                }
                else
                {
                    maxScore = Math.Min(maxScore, score);
                    beta = Math.Min(beta, maxScore);
                }

                if (beta <= alpha)
                    break;
            }

            return maxScore;
        }

        private int Evaluate(Board board)
        {
            if (board.GameOver != null)
                return EvaluateGameEnd(board);

            int score = 0;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    PieceBase piece = board.GetPiece(new Position(i, j));
                    if (piece != null)
                    {
                        score += EvaluatePieceWeights(piece, i, j);
                    }
                }
            }

            return score;
        }

        private int EvaluateGameEnd(Board board)
        {
            return board.GameOver.Winner switch
            {
                PieceColor.Black => 1000,
                PieceColor.White => -1000,
                _ => 0
            };
        }

        private int EvaluatePieceWeights(PieceBase piece, int row, int col)
        {
            int pieceValue = pieceWeights[piece.Type];
            bool isWhite = piece.Color == PieceColor.White;

            if (isWhite)
                pieceValue = -pieceValue;

            if (IsMiddleSquare(row, col) && piece.Type == ChessPieceType.Pawn)
                pieceValue += isWhite 
                    ? -centralBonus 
                    : centralBonus;

            return pieceValue;
        }

        private bool IsMiddleSquare(int row, int col)
        {
            return row >= 3 && row <= 4 && col >= 3 && col <= 4;
        }
    }
}
