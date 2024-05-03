namespace ChessManagementClasses
{
    public class EnPassantMove : MoveBase
    {
        protected Position capturedPawnPosition;
        protected RegularMove pawnMove;

        public EnPassantMove(Position start, Position end) : base(start, end)
        {
            capturedPawnPosition = new Position(start.Rank, end.File);
        }

        public override void MakeMove(Board board)
        {
            pawnMove = new RegularMove(StartPosition, EndPosition);
            pawnMove.MakeMove(board);
            capturedPiece = board.GetPiece(capturedPawnPosition);
            board.SetPiece(capturedPawnPosition, null);
        }

        public override void ReverseMove(Board board)
        {
            pawnMove.ReverseMove(board);
            board.SetPiece(capturedPawnPosition, capturedPiece);
        }
    }
}
