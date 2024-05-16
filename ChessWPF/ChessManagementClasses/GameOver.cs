namespace ChessManagementClasses
{
	public class GameOver
	{
		public PieceColor? Winner { get; }
		public PossibleEndings Ending { get; }

		public GameOver(PieceColor? winner, PossibleEndings ending)
		{
			Winner = winner;
			Ending = ending;
		}

		public GameOver(GameOver other)
		{
            Winner = other.Winner;
            Ending = other.Ending;
        }
	}
}
