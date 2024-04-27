using ChessManagementClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

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

			if (CanCastle(board, PositionChanges.Right))
				moves.Add(new CastlingMove(PositionChanges.Right, new Position(position)));

			if (CanCastle(board, PositionChanges.Left))
				moves.Add(new CastlingMove(PositionChanges.Left, new Position(position)));

			return moves;
		}

		protected bool CanCastle(Board board, PositionChanges positionChange)
		{
			if (HasMoved)
                return false;

			Position rookPosition;
			Position kingPosition;

            if (positionChange == PositionChanges.Right)
			{
				if (Color == PieceColor.White)
				{
					rookPosition = new Position(7, 7);
					kingPosition = new Position(7, 4);
				}
				else
				{
					rookPosition = new Position(0, 7);
                    kingPosition = new Position(0, 4);
                }
            }
			else
			{
				if (Color == PieceColor.White)
				{
					rookPosition = new Position(7, 0);
					kingPosition = new Position(7, 4);
				}
				else
				{
					rookPosition = new Position(0, 0);
					kingPosition = new Position(0, 4);
				}
			}

            if (board.IsPositionEmpty(rookPosition) || board.GetPiece(rookPosition).HasMoved)
                return false;

			return true;
        }
	}
}
