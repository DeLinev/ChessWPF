using ChessManagementClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessManagementClasses
{
	public class Bishop : PieceBase
	{
		public override ChessPieceType Type => ChessPieceType.Bishop;

		public Bishop(PieceColor color) : base(color) { }

		public Bishop(Bishop obj) : base(obj.Color) { }

		public override List<MoveBase> GetPossibleMoves(Board board, Position position)
		{
			List<MoveBase> moves = new List<MoveBase>();
			Position current = new Position(position);

			// Adds all possible moves for the UpLeft direction
			foreach (var nextPos in PossibleMovesInDirection(board, current, PositionChanges.UpLeft))
				moves.Add(new RegularMove(new Position(position), nextPos));

			// Adds all possible moves for the UpRight direction
			current = new Position(position);
			foreach (var nextPos in PossibleMovesInDirection(board, current, PositionChanges.UpRight))
				moves.Add(new RegularMove(new Position(position), nextPos));

			// Adds all possible moves for the DownLeft direction
			current = new Position(position);
			foreach (var nextPos in PossibleMovesInDirection(board, current, PositionChanges.DownLeft))
				moves.Add(new RegularMove(new Position(position), nextPos));

			// Adds all possible moves for the DownRight direction
			current = new Position(position);
			foreach (var nextPos in PossibleMovesInDirection(board, current, PositionChanges.DownRight))
				moves.Add(new RegularMove(new Position(position), nextPos));

			return moves;
		}
	}
}
