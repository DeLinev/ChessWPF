namespace ChessManagementClasses
{
    class CastlingMove : MoveBase
    {
        public PositionChanges castlingDirection;
        protected Position rookStart;
        protected Position rookEnd;
        protected RegularMove kingMove;
        protected RegularMove rookMove;

        public CastlingMove(PositionChanges posChange, Position kingPosition)
        {
            StartPosition = kingPosition;
            EndPosition = new Position(StartPosition.Rank, StartPosition.ChangePosition(posChange, 2).File);

            if (posChange == PositionChanges.Right)
            {
                castlingDirection = PositionChanges.Right;
                rookStart = new Position(StartPosition.Rank, 7);
                rookEnd = new Position(StartPosition.Rank, rookStart.ChangePosition(posChange, -2).File);
            }
            else
            {
                castlingDirection = PositionChanges.Left;
                rookStart = new Position(StartPosition.Rank, 0);
                rookEnd = new Position(StartPosition.Rank, rookStart.ChangePosition(posChange, -3).File);
            }
        }

        public override void MakeMove(Board board)
        {
            kingMove = new RegularMove(StartPosition, EndPosition);
            rookMove = new RegularMove(rookStart, rookEnd);

            kingMove.MakeMove(board);
            rookMove.MakeMove(board);
        }

        public override void ReverseMove(Board board)
        {
            if (kingMove == null || rookMove == null)
                return;

            rookMove.ReverseMove(board);
            kingMove.ReverseMove(board);
        }

        public override bool IsValid(Board board)
        {
            if (board.IsCheck())
                return false;

            for (int i = 1; i < 3; i++)
            {
                Position current = new Position(StartPosition.Rank, StartPosition.ChangePosition(castlingDirection, i).File);
                RegularMove move = new RegularMove(StartPosition, current);

                if (!board.IsPositionEmpty(current))
                    return false;

                move.MakeMove(board);

                if (board.IsCheck())
                {
                    move.ReverseMove(board);
                    return false;
                }

                move.ReverseMove(board);
            }

            return true;
        }
    }
}
