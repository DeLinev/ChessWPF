using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessManagementClasses
{
	public class RegularMove : MoveBase
	{
		public override MovesTypes Type { get => MovesTypes.Regular; }
		protected PieceBase capturedPiece;
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
