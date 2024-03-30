using ChessManagementClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessManagementClasses
{
	public class King : PieceBase
	{
		public override ChessPieceType Type => ChessPieceType.King;

		public King(PieceColor color) : base(color) { }

		public King(King obj) : base(obj.Color) { }

		public override List<MoveBase> GetPossibleMoves(Board board, Position position)
		{
			List<MoveBase> moves = new List<MoveBase>();
			Position current;

			foreach (var direction in new PositionChanges[] { PositionChanges.Up, PositionChanges.UpRight, PositionChanges.Right, PositionChanges.DownRight,
															  PositionChanges.Down, PositionChanges.DownLeft, PositionChanges.Left, PositionChanges.UpLeft })
			{
				current = position.ChangePosition(direction);

				if (Board.IsPositionValid(current) && (board.IsPositionEmpty(current) || board.GetPiece(current).Color != Color))
					moves.Add(new RegularMove(new Position(position), new Position(current)));
			}

			return moves;
		}
	}
}
