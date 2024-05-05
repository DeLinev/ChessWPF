namespace ChessManagementClasses
{
    public class King : PieceBase
	{
		public override ChessPieceType Type => ChessPieceType.King;

		public King(PieceColor color) : base(color) { }
        public King(King k) : base(k) { }

        public override List<MoveBase> GetPossibleMoves(Board board, Position position)
		{
			List<MoveBase> moves = new List<MoveBase>();
			Position current;

			foreach (var direction in new PositionChanges[] { PositionChanges.Up, PositionChanges.UpRight, PositionChanges.Right, PositionChanges.DownRight,
															  PositionChanges.Down, PositionChanges.DownLeft, PositionChanges.Left, PositionChanges.UpLeft })
			{
				if ((direction == PositionChanges.Right || direction == PositionChanges.Left) && CanCastle(board, direction))
                    moves.Add(new CastlingMove(direction, position));

                current = position.ChangePosition(direction);
				if (Board.IsPositionValid(current) && (board.IsPositionEmpty(current) || board.GetPiece(current).Color != Color))
					moves.Add(new RegularMove(position, current));
			}

			return moves;
		}

		protected bool CanCastle(Board board, PositionChanges positionChange)
		{
			if (HasMoved)
                return false;

			Position rookPosition;

            if (positionChange == PositionChanges.Right)
			{
				if (Color == PieceColor.White)
					rookPosition = new Position(7, 7);
				else
					rookPosition = new Position(0, 7);
            }
			else
			{
				if (Color == PieceColor.White)
					rookPosition = new Position(7, 0);
				else
					rookPosition = new Position(0, 0);
			}

            if (board.IsPositionEmpty(rookPosition) || board.GetPiece(rookPosition).HasMoved)
                return false;

			return true;
        }
	}
}
