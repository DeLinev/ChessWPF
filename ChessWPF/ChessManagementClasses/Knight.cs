namespace ChessManagementClasses
{
    public class Knight : PieceBase
	{
		public override ChessPieceType Type { get => ChessPieceType.Knight; }
        public override string ImagePath { get => Color == PieceColor.White
                                                        ? "/Assets/WhiteKnight.png"
                                                        : "/Assets/BlackKnight.png"; }
        public Knight(PieceColor color) : base(color) { }
		public Knight(Knight k) : base(k) { }
        public override PieceBase Clone()
        {
            return new Knight(this);
        }
        public override List<MoveBase> GetPossibleMoves(Board board, Position position)
		{
			List<MoveBase> moves = new List<MoveBase>();

			foreach (var direction in new PositionChanges[] { PositionChanges.Up, PositionChanges.Right, PositionChanges.Down, PositionChanges.Left })
			{
				foreach (var nextPos in PossibleMovesInDirection(board, position, direction))
					moves.Add(new RegularMove(new Position(position), nextPos));
			}

			return moves;
		}

		protected override IEnumerable<Position> PossibleMovesInDirection(Board board, Position position, PositionChanges mainDirection)
		{
			Position current;
			PositionChanges[] dirs;

            if (mainDirection == PositionChanges.Up || mainDirection == PositionChanges.Down)
                dirs = [PositionChanges.Right, PositionChanges.Left];
            else
                dirs = [PositionChanges.Up, PositionChanges.Down];

            foreach (var dir in dirs)
            {
                current = position.ChangePosition(mainDirection, 2).ChangePosition(dir);
                if (Board.IsPositionValid(current) && (board.IsPositionEmpty(current) || board.GetPiece(current).Color != Color))
                    yield return current;
            }

            yield break;
		}
	}
}
