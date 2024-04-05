using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
	}
}
