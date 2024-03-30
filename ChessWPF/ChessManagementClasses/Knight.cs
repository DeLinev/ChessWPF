using ChessManagementClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessManagementClasses
{
	public class Knight : PieceBase
	{
		public override ChessPieceType Type { get => ChessPieceType.Knight; }

		public Knight(PieceColor color) : base(color) { }

		public Knight(Knight obj) : base(obj.Color) { }

		public override List<MoveBase> GetPossibleMoves(Board board, Position position)
		{
			List<MoveBase> moves = new List<MoveBase>();

			foreach (var direction in new PositionChanges[] { PositionChanges.Up, PositionChanges.Right, PositionChanges.Down, PositionChanges.Left })
			{
				foreach (var nextPos in PossibleMovesInDirection(board, position, direction))
					moves.Add(new RegularMove(new Position(position), nextPos));
			}

			return moves;
		}

		protected override IEnumerable<Position> PossibleMovesInDirection(Board board, Position position, PositionChanges direction)
		{
			Position current;

			if (direction == PositionChanges.Up || direction == PositionChanges.Down)
			{
				current = position.ChangePosition(direction, 2).ChangePosition(PositionChanges.Right);
				if (Board.IsPositionValid(current) && (board.IsPositionEmpty(current) || board.GetPiece(current).Color != Color))
					yield return new Position(current);

				current = position.ChangePosition(direction, 2).ChangePosition(PositionChanges.Left);
				if (Board.IsPositionValid(current) && (board.IsPositionEmpty(current) || board.GetPiece(current).Color != Color))
					yield return new Position(current);
			}
			else
			{
				current = position.ChangePosition(direction, 2).ChangePosition(PositionChanges.Up);
				if (Board.IsPositionValid(current) && (board.IsPositionEmpty(current) || board.GetPiece(current).Color != Color))
					yield return new Position(current);

				current = position.ChangePosition(direction, 2).ChangePosition(PositionChanges.Down);
				if (Board.IsPositionValid(current) && (board.IsPositionEmpty(current) || board.GetPiece(current).Color != Color))
					yield return new Position(current);
			}

			yield break;
		}
	}
}
