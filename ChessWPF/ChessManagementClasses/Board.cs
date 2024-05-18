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

        public Board(Board other)
        {
			for (int i = 0; i < 8; i++)
			{
				for (int j = 0; j < 8; j++)
				{
					if (other.pieces[i, j] != null)
						pieces[i, j] = other.pieces[i, j].Clone();
				}
			}

            currentPlayer = other.currentPlayer;
            whitePiecesNum = new Dictionary<ChessPieceType, byte>(other.whitePiecesNum);
            blackPiecesNum = new Dictionary<ChessPieceType, byte>(other.blackPiecesNum);
            totalPiecesNum = other.totalPiecesNum;
            fiftyMoveRuleCounter = other.fiftyMoveRuleCounter;
            EnPassantPosition = other.EnPassantPosition != null ? new Position(other.EnPassantPosition) : null;
            GameOver = other.GameOver != null ? new GameOver(other.GameOver) : null;
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

		public void SetBoard(string fenStr)
		{
			pieces = new PieceBase[8, 8];

			string board = fenStr.Split(' ')[0];
			string currPl = fenStr.Split(' ')[1];
			string castling = fenStr.Split(' ')[2];
			string enPassant = fenStr.Split(' ')[3];

			int rank = -1;

			foreach (string row in board.Split('/'))
			{
				int file = 0;
				rank++;

				foreach (char c in row)
				{
					if (char.IsDigit(c))
					{
						file += int.Parse(c.ToString());
						continue;
					}

					PieceBase piece = null;

					switch (c)
					{
						case 'p':
							piece = new Pawn(PieceColor.Black);

							if (rank != 1)
								piece.HasMoved = true;

							break;
						case 'r':
							piece = new Rook(PieceColor.Black);
							piece.HasMoved = true;
							break;
						case 'n':
							piece = new Knight(PieceColor.Black);
							break;
						case 'b':
							piece = new Bishop(PieceColor.Black);
							break;
						case 'q':
							piece = new Queen(PieceColor.Black);
							break;
						case 'k':
							piece = new King(PieceColor.Black);
							piece.HasMoved = true;
							break;
						case 'P':
							piece = new Pawn(PieceColor.White);

							if (rank != 6)
								piece.HasMoved = true;

							break;
						case 'R':
							piece = new Rook(PieceColor.White);
							piece.HasMoved = true;
							break;
						case 'N':
							piece = new Knight(PieceColor.White);
							break;
						case 'B':
							piece = new Bishop(PieceColor.White);
							break;
						case 'Q':
							piece = new Queen(PieceColor.White);
							break;
						case 'K':
							piece = new King(PieceColor.White);
							piece.HasMoved = true;
							break;
					}

					if (piece != null)
					{
						pieces[rank, file % 8] = piece;
						file++;
					}
				}
			}

			currentPlayer = currPl == "w" ? PieceColor.White : PieceColor.Black;

			foreach (char c in castling)
			{
				switch (c)
				{
					case 'K':
						pieces[7, 7].HasMoved = false;
						pieces[7, 4].HasMoved = false;
						break;
					case 'Q':
						pieces[7, 0].HasMoved = false;
						pieces[7, 4].HasMoved = false;
						break;
					case 'k':
						pieces[0, 7].HasMoved = false;
						pieces[0, 4].HasMoved = false;
						break;
					case 'q':
						pieces[0, 0].HasMoved = false;
						pieces[0, 4].HasMoved = false;
						break;
				}
			}

			if (enPassant != "-")
				EnPassantPosition = new Position(enPassant);
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

		public string GetFenString()
		{
			string fnStr = "";

			for (int i = 0; i < 8; i++)
			{
				if (i != 0)
					fnStr += "/";

				int emptyCount = 0;

				for (int j = 0; j < 8; j++)
				{
					if (pieces[i, j] == null)
					{
						emptyCount++;
						continue;
					}

					if (emptyCount > 0)
					{
						fnStr += emptyCount.ToString();
						emptyCount = 0;
					}

					fnStr += pieces[i, j].GetPieceChar();
				}

				if (emptyCount > 0)
					fnStr += emptyCount.ToString();
			}

			fnStr += " ";
			
			if (currentPlayer == PieceColor.White)
				fnStr += "w";
			else
				fnStr += "b";

			fnStr += " ";
			
			bool canWCastleR = CanCastleRight(PieceColor.White);
			bool canWCastleL = CanCastleLeft(PieceColor.White);
			bool canBCastleR = CanCastleRight(PieceColor.Black);
			bool canBCastleL = CanCastleLeft(PieceColor.Black);

			if (canWCastleR || canWCastleL || canBCastleR || canBCastleL)
			{
				if (canWCastleR)
					fnStr += "K";
				if (canWCastleL)
					fnStr += "Q";
				if (canBCastleR)
					fnStr += "k";
				if (canBCastleL)
					fnStr += "q";
			}
			else
				fnStr += "-";

			fnStr += " ";
			
			if (EnPassantPosition != null)
				fnStr += EnPassantPosition.ToString();
			else
				fnStr += "-";

			return fnStr;
		}

		protected bool CanCastleRight(PieceColor color)
		{
			Position rookPosition;
			Position kingPosition;

			if (color == PieceColor.White)
			{
				rookPosition = new Position(7, 7);
				kingPosition = new Position(7, 4);
			}
			else
			{
				rookPosition = new Position(0, 7);
				kingPosition = new Position(0, 4);
			}

			if (IsPositionEmpty(rookPosition) || GetPiece(rookPosition).HasMoved)
				return false;

			if (IsPositionEmpty(kingPosition) || GetPiece(kingPosition).HasMoved)
				return false;

			return true;
		}

		protected bool CanCastleLeft(PieceColor color)
		{
			Position rookPosition;
			Position kingPosition;

			if (color == PieceColor.White)
			{
				rookPosition = new Position(7, 0);
				kingPosition = new Position(7, 4);
			}
			else
			{
				rookPosition = new Position(0, 0);
				kingPosition = new Position(0, 4);
			}

			if (IsPositionEmpty(rookPosition) || GetPiece(rookPosition).HasMoved)
				return false;

			if (IsPositionEmpty(kingPosition) || GetPiece(kingPosition).HasMoved)
				return false;

			return true;
		}
	}
}
