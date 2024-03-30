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

		public RegularMove(Position start, Position end) : base(start, end) { }

		public override void MakeMove(Board board)
		{
			PieceBase piece = board.GetPiece(StartPosition);

			board.SetPiece(EndPosition, piece);
			board.SetPiece(StartPosition, null);

			piece.HasMoved = true;
		}
	}
}
