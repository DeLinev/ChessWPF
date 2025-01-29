namespace ChessManagementClasses
{
    public class Bishop : PieceBase
	{
		public override ChessPieceType Type => ChessPieceType.Bishop;
        public override string ImagePath { get => Color == PieceColor.White
                                                        ? "/Assets/WhiteBishop.png"
                                                        : "/Assets/BlackBishop.png"; }
        public Bishop(PieceColor color) : base(color) { }
        public Bishop(Bishop b) : base(b) { }
        public override PieceBase Clone()
        {
            return new Bishop(this);
        }
        public override List<MoveBase> GetPossibleMoves(Board board, Position position)
		{
			List<MoveBase> moves = new List<MoveBase>();

			foreach (var direction in new PositionChanges[] { PositionChanges.UpLeft, PositionChanges.UpRight, PositionChanges.DownLeft, PositionChanges.DownRight })
			{
                foreach (var nextPos in PossibleMovesInDirection(board, position, direction))
                    moves.Add(new RegularMove(position, nextPos));
            }

			return moves;
		}
	}
}
