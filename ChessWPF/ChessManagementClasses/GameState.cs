namespace ChessManagementClasses
{
    public class GameState
    {
        public PieceColor CurrentPlayer { get; private set; } = PieceColor.White;
        public Position EnPassantPosition { get; set; }
        public byte FiftyMoveRuleCounter { get; private set; }

        public GameOver GameOver { get; private set; }

        public GameState()
        {
            CurrentPlayer = PieceColor.White;
            FiftyMoveRuleCounter = 0;
        }

        public void SwitchPlayer()
        {
            CurrentPlayer = CurrentPlayer == PieceColor.White ? PieceColor.Black : PieceColor.White;
        }

        public void ResetFiftyMoveRule() => FiftyMoveRuleCounter = 0;

        public void IncrementFiftyMoveRule() => FiftyMoveRuleCounter++;

        public void SetGameOver(PossibleEndings ending, PieceColor? winner = null)
        {
            GameOver = new GameOver(winner, ending);
        }
    }
}
