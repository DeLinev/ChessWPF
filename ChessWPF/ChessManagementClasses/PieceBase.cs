namespace ChessManagementClasses
{
    public abstract class PieceBase
    {
        protected bool previeousHasMoved;
        protected bool hasMoved = false;

        public abstract ChessPieceType Type { get; }
        public PieceColor Color { get; }
        public bool HasMoved
        {
            get => hasMoved;
            set
            {
                previeousHasMoved = hasMoved;
                hasMoved = value;
            }
        }
        public bool PreviousHasMoved { get => previeousHasMoved; }

        public PieceBase(PieceColor color)
        {
            Color = color;
        }

        public abstract List<MoveBase> GetPossibleMoves(Board board, Position current);

        public virtual bool IsThreatToKing(Board board, Position current)
        {
            List<MoveBase> moves = GetPossibleMoves(board, current);

            foreach (MoveBase move in moves)
            {
                if (board.GetPiece(move.EndPosition) != null && board.GetPiece(move.EndPosition).Type == ChessPieceType.King)
                    return true;
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
                yield return current;

            yield break;
        }
    }
}
