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
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    PieceBase piece = board.GetPiece(new Position(i, j));

                    if (piece != null && piece.Color == board.CurrentPlayer)
                        possibleMoves.AddRange(board.GetPossibleMoves(new Position(i, j)));
                }
            }

            MoveBase chosenMove = null;
            int bestScore = int.MinValue;

            foreach (MoveBase move in possibleMoves)
            {
                move.MakeMove(board);
                int score = Evaluate(board);
                move.ReverseMove(board);

                if (score > bestScore)
                {
                    bestScore = score;
                    chosenMove = move;
                }
            }

            BestMove = chosenMove;
            BestMove.MakeMove(board);
            CapturedPiece = BestMove.CapturedPiece;
        }

        public override void ReverseMove(Board board)
        {
            bestMove.ReverseMove(board);
        }

        protected int Evaluate(Board board)
        {
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
                    Position position = new Position(i, j);

                    if (board.IsPositionEmpty(position))
                        continue;

                    PieceBase piece = board.GetPiece(position);
                    int value = pieceValues[piece.Type];

                    score += piece.Color == board.CurrentPlayer ? value : -value;

                    if (piece.Type == ChessPieceType.Pawn || piece.Type == ChessPieceType.Knight)
                    {
                        if (i >= 3 && i <= 5 && j >= 3 && j <= 5)
                            score += piece.Color == board.CurrentPlayer ? 1 : -1;
                    }
                }
            }

            return score;
        }
    }
}
