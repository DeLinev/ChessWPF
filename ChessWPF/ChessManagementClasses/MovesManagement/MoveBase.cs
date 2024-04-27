using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessManagementClasses
{
	public abstract class MoveBase
	{
		public Position StartPosition { get; protected set; }
		public Position EndPosition { get; protected set; }

		public MoveBase(Position start, Position end)
		{
			StartPosition = start;
			EndPosition = end;
		}

		public MoveBase()
		{
            StartPosition = new Position(0, 0);
            EndPosition = new Position(0, 0);
        }

		public abstract void MakeMove(Board board);

		public abstract void ReverseMove(Board board);

		public virtual bool IsValid(Board board)
		{
			MakeMove(board);
			bool result = !board.IsCheck();
			ReverseMove(board);

			return result;
		}
	}
}
