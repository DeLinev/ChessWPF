using ChessManagementClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessManagementClasses
{
	public abstract class PieceBase
	{
		public abstract ChessPieceType Type { get; }
		public PieceColor Color { get; }
		public bool HasMoved { get; set; } = false;

		public PieceBase(PieceColor color)
		{
			Color = color;
		}

		public abstract List<MoveBase> GetPossibleMoves(Board board, Position current);

		protected virtual IEnumerable<Position> PossibleMovesInDirection(Board board, Position current, PositionChanges direction)
		{
			current = current.ChangePosition(direction);

			while (Board.IsPositionValid(current) && board.IsPositionEmpty(current))
			{
				yield return new Position(current);
				current = current.ChangePosition(direction);
			}

			if (Board.IsPositionValid(current) && board.GetPiece(current).Color != Color)
				yield return new Position(current);

			yield break;
		}
	}
}
