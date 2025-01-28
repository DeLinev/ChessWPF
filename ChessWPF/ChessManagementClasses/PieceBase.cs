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

        public abstract string ImagePath { get; }

        public PieceBase(PieceColor color)
        {
            Color = color;
        }

        public PieceBase(PieceBase other)
        {
            Color = other.Color;
            hasMoved = other.hasMoved;
            previeousHasMoved = other.previeousHasMoved;
        }

        public PieceBase Clone()
        {
            switch(Type)
            {
                case ChessPieceType.Pawn:
                    return new Pawn(this as Pawn);
                case ChessPieceType.Rook:
                    return new Rook(this as Rook);
                case ChessPieceType.Knight:
                    return new Knight(this as Knight);
                case ChessPieceType.Bishop:
                    return new Bishop(this as Bishop);
                case ChessPieceType.Queen:
                    return new Queen(this as Queen);
                case ChessPieceType.King:
                    return new King(this as King);
                default:
                    throw new Exception("Invalid piece type");
            }
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

        public char GetPieceChar()
        {
            char pieceChar = ' ';

            switch (Type)
            {
                case ChessPieceType.Pawn:
					pieceChar = 'p';
					break;
                case ChessPieceType.Rook:
                    pieceChar = 'r';
                    break;
                case ChessPieceType.Knight:
					pieceChar = 'n';
					break;
                case ChessPieceType.Bishop:
                    pieceChar = 'b';
                    break;
                case ChessPieceType.Queen:
					pieceChar = 'q';
					break;
                case ChessPieceType.King:
					pieceChar = 'k';
					break;
            }

            return Color == PieceColor.White ? char.ToUpper(pieceChar) : pieceChar;
        }
    }
}
