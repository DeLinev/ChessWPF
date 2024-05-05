namespace ChessManagementClasses
{
    public class Rook : PieceBase
	{
		public override ChessPieceType Type => ChessPieceType.Rook;

		public Rook(PieceColor color) : base(color) { }

		public Rook(Rook r) : base(r) { }

		public override List<MoveBase> GetPossibleMoves(Board board, Position position)
		{
			List<MoveBase> moves = new List<MoveBase>();

			foreach (var direction in new PositionChanges[] { PositionChanges.Up, PositionChanges.Right, PositionChanges.Down, PositionChanges.Left })
			{
                foreach (var nextPos in PossibleMovesInDirection(board, position, direction))
                    moves.Add(new RegularMove(position, nextPos));
            }

			return moves;
		}
	}
}
