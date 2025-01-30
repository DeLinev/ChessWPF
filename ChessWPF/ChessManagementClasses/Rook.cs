namespace ChessManagementClasses
{
    public class Rook : PieceBase
	{
		public override ChessPieceType Type => ChessPieceType.Rook;
        public override string ImagePath { get => Color == PieceColor.White
														? "/Assets/WhiteRook.png"
                                                        : "/Assets/BlackRook.png"; }
        public Rook(PieceColor color) : base(color) { }
		public Rook(Rook r) : base(r) { }
        public override PieceBase Clone()
        {
            return new Rook(this);
        }
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
