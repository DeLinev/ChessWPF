using System.Collections.Generic;

namespace ChessManagementClasses
{
    public class GameRules
    {
        public static bool IsCheck(ChessBoard board, GameState state)
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    PieceBase piece = board.GetPiece(new Position(i, j));
                    if (piece != null && piece.Color != state.CurrentPlayer)
                    {
                        if (piece.IsThreatToKing(board, new Position(i, j)))
                        {
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public static bool IsStalemate(ChessBoard board, GameState state)
        {
            List<MoveBase> moves = new List<MoveBase>();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    PieceBase piece = board.GetPiece(new Position(i, j));
                    if (piece != null && piece.Color == state.CurrentPlayer)
                    {
                        moves.AddRange(piece.GetPossibleMoves(board, new Position(i, j)));
                    }
                }
            }
            return moves.Count == 0;
        }

        public static bool IsInsufficientMaterial(ChessBoard board)
        {
            int totalPieces = 0;
            int bishops = 0;
            int knights = 0;

            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    PieceBase piece = board.GetPiece(new Position(i, j));
                    if (piece != null)
                    {
                        totalPieces++;
                        if (piece.Type == ChessPieceType.Bishop) bishops++;
                        if (piece.Type == ChessPieceType.Knight) knights++;
                    }
                }
            }

            if (totalPieces == 2) return true;
            if (totalPieces == 3 && (bishops == 1 || knights == 1)) return true;
            return false;
        }
    }
}
