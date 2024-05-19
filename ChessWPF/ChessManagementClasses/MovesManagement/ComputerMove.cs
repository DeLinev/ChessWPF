namespace ChessManagementClasses
{
    public class ComputerMove : MoveBase
    {
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

            MoveBase chosenMove = null;
            int bestScore = int.MinValue;
            int alpha = int.MinValue;
            int beta = int.MaxValue;

            foreach (MoveBase move in possibleMoves)
            {
                Board boardCopy = new Board(board);
                boardCopy.Move(move);
                int score = Minimax(boardCopy, 2, alpha, beta, false);

                if (score > bestScore)
                {
                    bestScore = score;
                    chosenMove = move;
                }

                alpha = Math.Max(alpha, bestScore);

                if (beta <= alpha)
                    break;
            }

            BestMove = chosenMove;
            BestMove.MakeMove(board);
            CapturedPiece = BestMove.CapturedPiece;
        }

        public override void ReverseMove(Board board)
        {
            bestMove.ReverseMove(board);
        }

        private int Minimax(Board board, int depth, int alpha, int beta, bool maximizingPlayer)
        {
            if (depth == 0 || board.GameOver != null)
                return Evaluate(board);

            if (maximizingPlayer)
            {
                int maxScore = int.MinValue;
                List<MoveBase> moves = GetMoves(board);

                foreach (MoveBase move in moves)
                {
                    Board boardCopy = new Board(board);
                    boardCopy.Move(move);
                    int score = Minimax(boardCopy, depth - 1, alpha, beta, false);
                    maxScore = Math.Max(maxScore, score);
                    alpha = Math.Max(alpha, score);
                    if (beta <= alpha)
                        break;
                }

                return maxScore;
            }
            else
            {
                int minScore = int.MaxValue;
                List<MoveBase> moves = GetMoves(board);

                foreach (MoveBase move in moves)
                {
                    Board boardCopy = new Board(board);
                    boardCopy.Move(move);
                    int score = Minimax(boardCopy, depth - 1, alpha, beta, true);
                    minScore = Math.Min(minScore, score);
                    beta = Math.Min(beta, score);
                    if (beta <= alpha)
                        break;
                }

                return minScore;
            }
        }

        protected int Evaluate(Board board)
        {
			if (board.GameOver != null && board.GameOver.Winner == PieceColor.Black)
				return int.MaxValue;
			else if (board.GameOver != null && board.GameOver.Winner == PieceColor.White)
				return int.MinValue;

			Dictionary<ChessPieceType, int> pieceValues = new Dictionary<ChessPieceType, int>
            {
                { ChessPieceType.Pawn, 1 },
                { ChessPieceType.Knight, 3 },
                { ChessPieceType.Bishop, 3 },
                { ChessPieceType.Rook, 5 },
                { ChessPieceType.Queen, 9 },
                { ChessPieceType.King, 100 }
            };

            int score = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    PieceBase piece = board.GetPiece(new Position(i, j));

                    if (piece != null)
                    {

                        if (piece.Color == PieceColor.Black)
                        {
                            switch (piece.Type)
                            {
                                case ChessPieceType.Pawn:
                                    score += 1;
                                    if (i >= 3 && i <= 4 && j >= 3 && j <= 4)
                                        score += 2;
                                    break;
                                case ChessPieceType.Knight:
                                case ChessPieceType.Bishop:
                                    score += 3;
                                    break;
                                case ChessPieceType.Rook:
                                    score += 5;
                                    break;
                                case ChessPieceType.Queen:
                                    score += 9;
                                    break;
                            }
                        }
                        else
                        {
                            switch (piece.Type)
                            {
                                case ChessPieceType.Pawn:
                                    score -= 1;
                                    if (i >= 3 && i <= 4 && j >= 3 && j <= 4)
                                        score -= 2;
                                    break;
                                case ChessPieceType.Knight:
                                case ChessPieceType.Bishop:
                                    score -= 3;
                                    break;
                                case ChessPieceType.Rook:
                                    score -= 5;
                                    break;
                                case ChessPieceType.Queen:
                                    score -= 9;
                                    break;
                            }
                        }
                    }
                }
            }

            return score;
        }

        protected List<MoveBase> GetMoves(Board board)
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
    }
}
