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
			Position current;

			if (Color == PieceColor.White) // White pawns
			{
				// Adds possible move for the Up direction
				current = position.ChangePosition(PositionChanges.Up);
				if (Board.IsPositionValid(current) && board.IsPositionEmpty(current))
					SelectMove(position, current, moves);

				// Adds possible move for the Upx2 direction
				Position oneUp = position.ChangePosition(PositionChanges.Up);
				current = position.ChangePosition(PositionChanges.Up, 2);
				if (Board.IsPositionValid(current) && board.IsPositionEmpty([current, oneUp]) && !this.HasMoved)
					moves.Add(new RegularMove(new Position(position), new Position(current)));

				// Adds possible moves for diagonals direction
				foreach (PositionChanges direction in new PositionChanges[] { PositionChanges.UpLeft, PositionChanges.UpRight })
				{
					current = position.ChangePosition(direction);
					if (Board.IsPositionValid(current) && !board.IsPositionEmpty(current) && board.GetPiece(current).Color != Color)
						SelectMove(position, current, moves);
				}
			}
			else // Black pawns
			{
				// Adds possible move for the Down direction
				current = position.ChangePosition(PositionChanges.Down);
				if (Board.IsPositionValid(current) && board.IsPositionEmpty(current))
					SelectMove(position, current, moves);

				// Adds possible move for the Downx2 direction
				Position oneDown = position.ChangePosition(PositionChanges.Down);
				current = position.ChangePosition(PositionChanges.Down, 2);
				if (Board.IsPositionValid(current) && board.IsPositionEmpty([current, oneDown]) && !this.HasMoved)
					moves.Add(new RegularMove(new Position(position), new Position(current)));

				// Adds possible moves for diagonals direction
				foreach (PositionChanges direction in new PositionChanges[] { PositionChanges.DownLeft, PositionChanges.DownRight })
				{
					current = position.ChangePosition(direction);
					if (Board.IsPositionValid(current) && !board.IsPositionEmpty(current) && board.GetPiece(current).Color != Color)
						SelectMove(position, current, moves);
				}
			}

			return moves;
		}

		protected void SelectMove(Position position, Position current, List<MoveBase> moves)
		{
			if (Color == PieceColor.White ? current.currentRank == 0 : current.currentRank == 7) // Promotion
			{
				//foreach (MoveBase move in PossiblePromotions(position, current))
				//	moves.Add(move);
				moves.Add(new PromotionMove(new Position(position), new Position(current)));
			}
			else
			{
				moves.Add(new RegularMove(new Position(position), new Position(current)));
			}
		}
	}
}
