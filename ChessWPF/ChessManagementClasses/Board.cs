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
		protected Dictionary<ChessPieceType, byte> whitePiecesNum;
		protected Dictionary<ChessPieceType, byte> blackPiecesNum;
        protected byte totalPiecesNum;
		protected byte fiftyMoveRuleCounter;

        public Position EnPassantPosition { get; set; }
		public PieceColor CurrentPlayer { get => currentPlayer; }
		public GameOver GameOver { get; set; }

		public Board() 
		{
			SetBoard();
			currentPlayer = PieceColor.White;

			whitePiecesNum = new Dictionary<ChessPieceType, byte>();
			blackPiecesNum = new Dictionary<ChessPieceType, byte>();

			foreach (ChessPieceType pieceType in new ChessPieceType[] { ChessPieceType.Pawn, ChessPieceType.Rook, ChessPieceType.Knight, 
				ChessPieceType.Bishop, ChessPieceType.Queen, ChessPieceType.King})
			{
				whitePiecesNum[pieceType] = 0;
				blackPiecesNum[pieceType] = 0;
            }

			totalPiecesNum = 0;
			fiftyMoveRuleCounter = 0;
		}

		public PieceBase GetPiece(Position position)
		{
			return pieces[position.Rank, position.File];
		}

		public void SetPiece(Position position, PieceBase piece)
		{
			pieces[position.Rank, position.File] = piece;
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

		public bool IsPositionEmpty(Position[] positions)
		{
			foreach (Position position in positions)
				if (!IsPositionEmpty(position))
					return false;

			return true;
		}

		public static bool IsPositionValid(Position position)
		{
			return position.Rank >= 0 && position.Rank < 8 &&
				position.File >= 0 && position.File < 8;
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
			EnPassantPosition = null;

			move.MakeMove(this);
			PieceColor opponent = currentPlayer;
			currentPlayer = currentPlayer == PieceColor.White ? PieceColor.Black : PieceColor.White;

			if (move.IsCapture || GetPiece(move.EndPosition).Type == ChessPieceType.Pawn)
				fiftyMoveRuleCounter = 0;
            else
                fiftyMoveRuleCounter++;

			if (isStalemate())
			{
				if (IsCheck())
					GameOver = new GameOver(opponent, PossibleEndings.CheckMate);
				else
					GameOver = new GameOver(null, PossibleEndings.StaleMate);
			}
			else if (IsInsuffMaterial())
                GameOver = new GameOver(null, PossibleEndings.InsuffMaterial);
			else if (IsFiftyMoveRule())
				GameOver = new GameOver(null, PossibleEndings.FiftyMoveRule);
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

		public bool IsInsuffMaterial()
		{
			CountPieces();

			if (totalPiecesNum == 2)
                return true;

			if (totalPiecesNum == 3)
			{
				if (GetPiecesNum(ChessPieceType.Bishop, PieceColor.White) == 1 ||
					GetPiecesNum(ChessPieceType.Bishop, PieceColor.Black) == 1)
					return true;

				if (GetPiecesNum(ChessPieceType.Knight, PieceColor.White) == 1 ||
                    GetPiecesNum(ChessPieceType.Knight, PieceColor.Black) == 1)
					return true;
            }

            if (totalPiecesNum == 4)
			{
                if (GetPiecesNum(ChessPieceType.Bishop, PieceColor.White) != 1 ||
                    GetPiecesNum(ChessPieceType.Bishop, PieceColor.Black) != 1)
                    return false;

				Position WhiteBishopPosition = new Position(0, 0);
				Position BlackBishopPosition = new Position(0, 0);

                for (int i = 0; i < 8; i++)
				{
					for (int j = 0; j < 8; j++)
					{
                        if (pieces[i, j] != null && pieces[i, j].Type == ChessPieceType.Bishop)
						{
							if (pieces[i, j].Color == PieceColor.White)
                                WhiteBishopPosition = new Position(i, j);
                            else
                                BlackBishopPosition = new Position(i, j);
						}
                    }
				}

				if ((WhiteBishopPosition.File + WhiteBishopPosition.Rank) % 2 == (BlackBishopPosition.File + BlackBishopPosition.Rank) % 2)
                    return true;
            }

            return false;
        }

		protected void CountPieces()
		{
            foreach (ChessPieceType pieceType in new ChessPieceType[] { ChessPieceType.Pawn, ChessPieceType.Rook, ChessPieceType.Knight,
                ChessPieceType.Bishop, ChessPieceType.Queen, ChessPieceType.King})
            {
                whitePiecesNum[pieceType] = 0;
                blackPiecesNum[pieceType] = 0;
            }

            totalPiecesNum = 0;

            for (int i = 0; i < 8; i++)
			{
                for (int j = 0; j < 8; j++)
				{
                    if (pieces[i, j] != null)
					{
                        totalPiecesNum++;

                        if (pieces[i, j].Color == PieceColor.White)
							whitePiecesNum[pieces[i, j].Type]++;
                        else
							blackPiecesNum[pieces[i, j].Type]++;
                    }
                }
            }
        }

		protected byte GetPiecesNum(ChessPieceType pieceType, PieceColor color)
		{
            if (color == PieceColor.White)
                return whitePiecesNum[pieceType];
            else
                return blackPiecesNum[pieceType];
        }

		protected bool IsFiftyMoveRule() => (fiftyMoveRuleCounter / 2) >= 50;
	}
}
