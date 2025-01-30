namespace ChessManagementClasses
{
    public abstract class PieceBase
    {
        protected bool previousHasMoved;
        protected bool hasMoved;

        private static readonly Dictionary<ChessPieceType, char> pieceCharMap = new()
        {
            { ChessPieceType.Pawn, 'p' },
            { ChessPieceType.Rook, 'r' },
            { ChessPieceType.Knight, 'n' },
            { ChessPieceType.Bishop, 'b' },
            { ChessPieceType.Queen, 'q' },
            { ChessPieceType.King, 'k' }
        };

        public abstract ChessPieceType Type { get; }
        public PieceColor Color { get; }
        public bool PreviousHasMoved { get => previousHasMoved; }
        public abstract string ImagePath { get; }
        public bool HasMoved
        {
            get => hasMoved;
            set
            {
                previousHasMoved = hasMoved;
                hasMoved = value;
            }
        }

        public PieceBase(PieceColor color)
        {
            Color = color;
        }

        public PieceBase(PieceBase other)
        {
            Color = other.Color;
            hasMoved = other.hasMoved;
            previousHasMoved = other.previousHasMoved;
        }

        public abstract PieceBase Clone();

        public abstract List<MoveBase> GetPossibleMoves(Board board, Position current);

        public virtual bool IsThreatToKing(Board board, Position current)
        {
            List<MoveBase> moves = GetPossibleMoves(board, current);

            foreach (MoveBase move in moves)
            {
                var piece = board.GetPiece(move.EndPosition);
                if (piece != null && piece.Type == ChessPieceType.King)
                {
                    return true;
                }
            }

            return false;
        }

        protected virtual IEnumerable<Position> PossibleMovesInDirection(Board board, Position current, PositionChanges direction)
        {
            current = current.ChangePosition(direction);

            while (Board.IsPositionValid(current) && board.IsPositionEmpty(current))
            {
                yield return current;
                current = current.ChangePosition(direction);
            }

            if (Board.IsPositionValid(current) && board.GetPiece(current).Color != Color)
            {
                yield return current;
            }

            yield break;
        }

        public char GetPieceChar()
        {
            var pieceChar = pieceCharMap[Type];

            return Color == PieceColor.White ? char.ToUpper(pieceChar) : pieceChar;
        }
    }
}
