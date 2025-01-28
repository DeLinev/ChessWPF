namespace ChessManagementClasses
{
    public class Pawn : PieceBase
    {
        public override ChessPieceType Type { get => ChessPieceType.Pawn; }

        public override string ImagePath { get => Color == PieceColor.White 
                                                        ? "/Assets/WhitePawn.png"
                                                        : "/Assets/BlackPawn.png"; }

        public Pawn(PieceColor color) : base(color) { }

        public Pawn(Pawn p) : base(p) { }

        public override List<MoveBase> GetPossibleMoves(Board board, Position position)
        {
            List<MoveBase> moves = new List<MoveBase>();
            Position current;
            PositionChanges dir;
            PositionChanges[] dirs;

            if (Color == PieceColor.White)
            {
                dir = PositionChanges.Up;
                dirs = [PositionChanges.UpLeft, PositionChanges.UpRight];
            }
            else
            {
                dir = PositionChanges.Down;
                dirs = [PositionChanges.DownLeft, PositionChanges.DownRight];
            }

            current = position.ChangePosition(dir);
            if (Board.IsPositionValid(current) && board.IsPositionEmpty(current))
                SelectMove(position, current, moves);

            Position oneStep = position.ChangePosition(dir);
            current = position.ChangePosition(dir, 2);
            if (Board.IsPositionValid(current) && board.IsPositionEmpty([current, oneStep]) && !HasMoved)
                moves.Add(new RegularMove(position, current));

            foreach (PositionChanges direction in dirs)
            {
                current = position.ChangePosition(direction);

                if (current.Equals(board.EnPassantPosition))
                {
                    moves.Add(new EnPassantMove(position, current));
                    continue;
                }

                if (Board.IsPositionValid(current) && !board.IsPositionEmpty(current) && board.GetPiece(current).Color != Color)
                    SelectMove(position, current, moves);
            }

            return moves;
        }

        protected void SelectMove(Position position, Position current, List<MoveBase> moves)
        {
            if (Color == PieceColor.White ? current.Rank == 0 : current.Rank == 7)
                moves.Add(new PromotionMove(position, current));
            else
                moves.Add(new RegularMove(position, current));
        }
    }
}
