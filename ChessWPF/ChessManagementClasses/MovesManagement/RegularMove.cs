namespace ChessManagementClasses
{
    public class RegularMove : MoveBase
	{
		protected PieceBase capturedPiece;

		public bool IsCapture { get => capturedPiece != null; }

		public RegularMove(Position start, Position end) : base(start, end) { }

		public override void MakeMove(Board board)
		{
			PieceBase piece = board.GetPiece(StartPosition);
			capturedPiece = board.GetPiece(EndPosition);

			board.SetPiece(EndPosition, piece);
			board.SetPiece(StartPosition, null);

			piece.HasMoved = true;
		}

		public override void ReverseMove(Board board)
		{
			PieceBase piece = board.GetPiece(EndPosition);

			board.SetPiece(StartPosition, piece);
			board.SetPiece(EndPosition, capturedPiece);

			piece.HasMoved = piece.PreviousHasMoved;
		}
	}
}
