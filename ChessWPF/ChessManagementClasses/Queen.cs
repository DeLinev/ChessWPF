namespace ChessManagementClasses
{
    public class Queen : PieceBase
	{
		public override ChessPieceType Type => ChessPieceType.Queen;
        public override string ImagePath { get => Color == PieceColor.White
                                                        ? "/Assets/WhiteQueen.png"
                                                        : "/Assets/BlackQueen.png"; }
        public Queen(PieceColor color) : base(color) { }
		public Queen(Queen q) : base(q) { }
        public override PieceBase Clone()
        {
            return new Queen(this);
        }
        public override List<MoveBase> GetPossibleMoves(Board board, Position position)
		{
			List<MoveBase> moves = new List<MoveBase>();

			foreach (var direction in new PositionChanges[] { PositionChanges.Up, PositionChanges.UpRight, PositionChanges.Right, 
				PositionChanges.DownRight, PositionChanges.Down, PositionChanges.DownLeft, PositionChanges.Left, PositionChanges.UpLeft })
			{
                foreach (var nextPos in PossibleMovesInDirection(board, position, direction))
                    moves.Add(new RegularMove(position, nextPos));
            }

			return moves;
		}
	}
}
