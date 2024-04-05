using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessManagementClasses
{
	public abstract class MoveBase
	{
		public abstract MovesTypes Type { get; }
		public Position StartPosition { get; }
		public Position EndPosition { get; }

		public MoveBase(Position start, Position end)
		{
			StartPosition = start;
			EndPosition = end;
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
