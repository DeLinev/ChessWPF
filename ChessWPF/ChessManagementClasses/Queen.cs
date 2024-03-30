using ChessManagementClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessManagementClasses
{
	public class Queen : PieceBase
	{
		public override ChessPieceType Type => ChessPieceType.Queen;

		public Queen(PieceColor color) : base(color) { }

		public Queen(Queen obj) : base(obj.Color) { }

		public override List<MoveBase> GetPossibleMoves(Board board, Position position)
		{
			List<MoveBase> moves = new List<MoveBase>();
			Position current = new Position(position);

			// Adds all possible moves for the Up direction
			foreach (var nextPos in PossibleMovesInDirection(board, current, PositionChanges.Up))
				moves.Add(new RegularMove(new Position(position), nextPos));

			// Adds all possible moves for the UpRight direction
			current = new Position(position);
			foreach (var nextPos in PossibleMovesInDirection(board, current, PositionChanges.UpRight))
				moves.Add(new RegularMove(new Position(position), nextPos));

			// Adds all possible moves for the Right direction
			current = new Position(position);
			foreach (var nextPos in PossibleMovesInDirection(board, current, PositionChanges.Right))
				moves.Add(new RegularMove(new Position(position), nextPos));

			// Adds all possible moves for the DownRight direction
			current = new Position(position);
			foreach (var nextPos in PossibleMovesInDirection(board, current, PositionChanges.DownRight))
				moves.Add(new RegularMove(new Position(position), nextPos));

			// Adds all possible moves for the Down direction
			current = new Position(position);
			foreach (var nextPos in PossibleMovesInDirection(board, current, PositionChanges.Down))
				moves.Add(new RegularMove(new Position(position), nextPos));

			// Adds all possible moves for the DownLeft direction
			current = new Position(position);
			foreach (var nextPos in PossibleMovesInDirection(board, current, PositionChanges.DownLeft))
				moves.Add(new RegularMove(new Position(position), nextPos));

			// Adds all possible moves for the Left direction
			current = new Position(position);
			foreach (var nextPos in PossibleMovesInDirection(board, current, PositionChanges.Left))
				moves.Add(new RegularMove(new Position(position), nextPos));

			// Adds all possible moves for the UpLeft direction
			foreach (var nextPos in PossibleMovesInDirection(board, current, PositionChanges.UpLeft))
				moves.Add(new RegularMove(new Position(position), nextPos));

			return moves;
		}
	}
}
