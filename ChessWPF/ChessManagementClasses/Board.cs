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

			if (piece == null)
				return new List<MoveBase>();

			return piece.GetPossibleMoves(this, position);
		}

		public void Move(MoveBase move)
		{
			move.MakeMove(this);
			currentPlayer = currentPlayer == PieceColor.White ? PieceColor.Black : PieceColor.White;
		}
	}
}
