namespace ChessManagementClasses
{
    public class PromotionMove : MoveBase
	{
        protected ChessPieceType newPiece;

		public string NewPiece { 
			get {
				switch(newPiece)
				{
					case ChessPieceType.Queen:
						return "Q";
					case ChessPieceType.Rook:
						return "R";
					case ChessPieceType.Bishop:
						return "B";
					case ChessPieceType.Knight:
						return "N";
					default:
						throw new ArgumentException("Invalid piece type");
				}
			} 
		}

		public PromotionMove(Position start, Position end, ChessPieceType piece) : base(start, end)
		{
			newPiece = piece;
		}

		public PromotionMove(Position start, Position end) : base(start, end) 
		{
			newPiece = ChessPieceType.Queen;
		}

		public override void MakeMove(Board board)
		{
			PieceBase piece = board.GetPiece(StartPosition);
			CapturedPiece = board.GetPiece(EndPosition);

			PieceBase selectedPiece = Promote(piece.Color);
			board.SetPiece(EndPosition, selectedPiece);
			board.SetPiece(StartPosition, null);

			selectedPiece.HasMoved = true; 
		}

		public override void ReverseMove(Board board)
		{
			PieceBase piece = new Pawn(board.CurrentPlayer);
			piece.HasMoved = true;

			board.SetPiece(StartPosition, piece);
			board.SetPiece(EndPosition, CapturedPiece);
		}

		protected PieceBase Promote(PieceColor color)
		{
			switch (newPiece)
			{
				case ChessPieceType.Queen:
					return new Queen(color);
				case ChessPieceType.Rook:
					return new Rook(color);
				case ChessPieceType.Bishop:
					return new Bishop(color);
				case ChessPieceType.Knight:
					return new Knight(color);
				default:
					throw new ArgumentException("Invalid piece type");
			}
		}
	}
}
