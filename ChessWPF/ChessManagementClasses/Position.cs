namespace ChessManagementClasses
{
    public class Position
	{
		public int Rank { get; }
		public int File { get; }

		public Position(int rank, int file)
		{
			Rank = rank;
			File = file;
		}

		public Position(Position obj)
		{
			Rank = obj.Rank;
			File = obj.File;
		}

		public Position ChangePosition(PositionChanges change, int k = 1)
		{
			switch (change)
			{
				case PositionChanges.Up:
					return new Position(Rank - 1 * k, File);
				case PositionChanges.Down:
					return new Position(Rank + 1 * k, File);
				case PositionChanges.Left:
					return new Position(Rank, File - 1 * k);
				case PositionChanges.Right:
					return new Position(Rank, File + 1 * k);
				case PositionChanges.UpLeft:
					return new Position(Rank - 1 * k, File - 1 * k);
				case PositionChanges.UpRight:
					return new Position(Rank - 1 * k, File + 1 * k);
				case PositionChanges.DownLeft:
					return new Position(Rank + 1 * k, File - 1 * k);
				case PositionChanges.DownRight:
					return new Position(Rank + 1 * k, File + 1 * k);
				default:
					throw new ArgumentException("Invalid position change");
			}
		}

		public override string ToString()
		{
			char[] files = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];

			return files[File] + (8 - Rank).ToString();
		}

		public string GetFileLetter()
		{
			char[] files = ['a', 'b', 'c', 'd', 'e', 'f', 'g', 'h'];

			return files[File].ToString();
		}

        public override bool Equals(object? obj)
        {
			if (obj is not Position position)
				return false;

            return Rank == position.Rank
				&& File == position.File;
        }

		public override int GetHashCode()
		{
            return HashCode.Combine(File, Rank);
        }
    }
}
