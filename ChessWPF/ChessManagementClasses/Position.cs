using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessManagementClasses
{
	public class Position
	{
		public int currentRank { get; }
		public int currentFile { get; }

		public Position(int rank, int file)
		{
			currentRank = rank;
			currentFile = file;
		}

		public Position(Position obj)
		{
			currentRank = obj.currentRank;
			currentFile = obj.currentFile;
		}

		public Position ChangePosition(PositionChanges change, int k = 1)
		{
			switch (change)
			{
				case PositionChanges.Up:
					return new Position(currentRank - 1 * k, currentFile);
				case PositionChanges.Down:
					return new Position(currentRank + 1 * k, currentFile);
				case PositionChanges.Left:
					return new Position(currentRank, currentFile - 1 * k);
				case PositionChanges.Right:
					return new Position(currentRank, currentFile + 1 * k);
				case PositionChanges.UpLeft:
					return new Position(currentRank - 1 * k, currentFile - 1 * k);
				case PositionChanges.UpRight:
					return new Position(currentRank - 1 * k, currentFile + 1 * k);
				case PositionChanges.DownLeft:
					return new Position(currentRank + 1 * k, currentFile - 1 * k);
				case PositionChanges.DownRight:
					return new Position(currentRank + 1 * k, currentFile + 1 * k);
				default:
					throw new ArgumentException("Invalid position change");
			}
		}
	}
}
