namespace ChessManagementClasses
{
	public class King : PieceBase
	{
		public override ChessPieceType Type => ChessPieceType.King;

		public override string ImagePath
		{
			get => Color == PieceColor.White
														? "/Assets/WhiteKing.png"
														: "/Assets/BlackKing.png";
		}

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
			if (HasMoved) return false;

			Position rookPosition = GetRookPosition(positionChange);

			return !(board.IsPositionEmpty(rookPosition) || board.GetPiece(rookPosition).HasMoved);
		}

		private Position GetRookPosition(PositionChanges positionChange)
		{
			int rank = (Color == PieceColor.White) ? 7 : 0;
			int file = (positionChange == PositionChanges.Right) ? 7 : 0;

			return new Position(rank, file);
		}

	}
}
