using ChessManagementClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessManagementClasses
{
	public class Pawn : PieceBase
	{
		public override ChessPieceType Type { get => ChessPieceType.Pawn; }

		public Pawn(PieceColor color) : base(color) { }

		public Pawn(Pawn obj) : base(obj.Color) { }

		public override List<MoveBase> GetPossibleMoves(Board board, Position position)
		{
			List<MoveBase> moves = new List<MoveBase>();

			// Adds possible move for the Up direction
			Position current = position.ChangePosition(PositionChanges.Up);

			if (Board.IsPositionValid(current) && board.IsPositionEmpty(current))
				moves.Add(new RegularMove(new Position(position), new Position(current)));

			// Adds possible move for the UpLeft direction
			current = position.ChangePosition(PositionChanges.UpLeft);

			if (Board.IsPositionValid(current) && !board.IsPositionEmpty(current) && board.GetPiece(current).Color != Color)
				moves.Add(new RegularMove(new Position(position), new Position(current)));

			// Adds possible move for the UpRight direction
			current = position.ChangePosition(PositionChanges.UpRight);

			if (Board.IsPositionValid(current) && !board.IsPositionEmpty(current) && board.GetPiece(current).Color != Color)
				moves.Add(new RegularMove(new Position(position), new Position(current)));

			return moves;
		}
	}
}
