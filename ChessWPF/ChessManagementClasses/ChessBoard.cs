using System;
using System.Collections.Generic;

namespace ChessManagementClasses
{
    public class ChessBoard
    {
        private readonly PieceBase[,] pieces = new PieceBase[8, 8];

        public PieceBase GetPiece(Position position) => pieces[position.Rank, position.File];

        public void SetPiece(Position position, PieceBase piece) => pieces[position.Rank, position.File] = piece;

        public bool IsPositionEmpty(Position position) => GetPiece(position) == null;

        public static bool IsPositionValid(Position position) =>
            position.Rank >= 0 && position.Rank < 8 &&
            position.File >= 0 && position.File < 8;

        public void SetInitialBoard()
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
    }
}
