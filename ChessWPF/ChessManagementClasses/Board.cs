using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChessManagementClasses
{
	public class Board
	{
		protected PieceBase[,] pieces = new PieceBase[8, 8];

		protected PieceColor currentPlayer;

		public PieceColor CurrentPlayer { get => currentPlayer; }
		public GameOver GameOver { get; set; }

		public Board() 
		{
			SetBoard();
			currentPlayer = PieceColor.White;
		}

		public PieceBase GetPiece(Position position)
		{
			return pieces[position.currentRank, position.currentFile];
		}

		public void SetPiece(Position position, PieceBase piece)
		{
			pieces[position.currentRank, position.currentFile] = piece;
		}

		public void SetBoard()
		{
			for (int i = 0; i < 8; i++)
			{
				pieces[1, i] = new Pawn(PieceColor.Black);
				pieces[6, i] = new Pawn(PieceColor.White);
			}

			pieces[0, 0] = new Rook(PieceColor.Black);
			pieces[0, 7] = new Rook(PieceColor.Black);
			pieces[7, 0] = new Rook(PieceColor.White);
			pieces[7, 7] = new Rook(PieceColor.White);

			pieces[0, 1] = new Knight(PieceColor.Black);
			pieces[0, 6] = new Knight(PieceColor.Black);
			pieces[7, 1] = new Knight(PieceColor.White);
			pieces[7, 6] = new Knight(PieceColor.White);

			pieces[0, 2] = new Bishop(PieceColor.Black);
			pieces[0, 5] = new Bishop(PieceColor.Black);
			pieces[7, 2] = new Bishop(PieceColor.White);
			pieces[7, 5] = new Bishop(PieceColor.White);

			pieces[0, 3] = new Queen(PieceColor.Black);
			pieces[7, 3] = new Queen(PieceColor.White);

			pieces[0, 4] = new King(PieceColor.Black);
			pieces[7, 4] = new King(PieceColor.White);
		}

		public bool IsPositionEmpty(Position position)
		{
			return GetPiece(position) == null;
		}

		public static bool IsPositionValid(Position position)
		{
			return position.currentRank >= 0 && position.currentRank < 8 &&
				position.currentFile >= 0 && position.currentFile < 8;
		}

		public List<MoveBase> GetPossibleMoves(Position position)
		{
			PieceBase piece = GetPiece(position);

			if (piece == null || piece.Color != CurrentPlayer)
				return new List<MoveBase>();

			List<MoveBase> possibleMoves = piece.GetPossibleMoves(this, position);

			return (
				from move in possibleMoves
				where move.IsValid(this)
				select move
			).ToList();
		}

		public void Move(MoveBase move)
		{
			move.MakeMove(this);
			PieceColor opponent = currentPlayer;
			currentPlayer = currentPlayer == PieceColor.White ? PieceColor.Black : PieceColor.White;

			if (isStalemate())
			{
				if (IsCheck())
					GameOver = new GameOver(opponent, PossibleEndings.CheckMate);
				else
					GameOver = new GameOver(null, PossibleEndings.StaleMate);
			}
		}

		public bool IsCheck()
		{
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					if (pieces[i, j] != null && pieces[i, j].Color != currentPlayer)
					{
						if (pieces[i, j].IsThreatToKing(this, new Position(i, j)))
							return true;
					}
				}
			}

			return false;
		}

		public bool isStalemate()
		{
			List<MoveBase> moves = new List<MoveBase>();

			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					if (pieces[i, j] != null && pieces[i, j].Color == currentPlayer)
					{
						GetPossibleMoves(new Position(i, j)).ForEach(moves.Add);
					}
				}
			}

			if (moves.Count == 0)
				return true;

			return false;
		}
	}
}
