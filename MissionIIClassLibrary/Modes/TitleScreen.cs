using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Modes
{
    public class TitleScreen : BaseGameMode
    {
        private int _countDown = Constants.TitleScreenRollCycles;
        private bool _releaseWaiting = true;
        private bool _firstCycle = true;

        public TitleScreen()
        {
            if (MissionIIGameBoard.HiScoreTable == null) // TODO: Do we really want this created here?
            {
                MissionIIGameBoard.HiScoreTable = new GameClassLibrary.Hiscore.HiScoreScreen(
                    new GameClassLibrary.Hiscore.HiScoreScreenDimensions
                    { TopEdgeY = 70, BottomEdgeY = 246, NamesLeftX = 10, ScoresRightX = 310 },  // TODO: screen dimension constants!
                    MissionIISprites.Font);
            }
        }

        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
        {
            if (_firstCycle)
            {
                MissionIISounds.Intro.Play();
                _firstCycle = false;
            }

            if (theKeyStates.Fire)
            {
                if (_releaseWaiting) return;
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new StartNewGame();
            }

            _releaseWaiting = false;

            if (_countDown > 0)
            {
                --_countDown;
            }
            else
            {
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new HiScoreMode();
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawSprite(0, 0, MissionIISprites.TitleScreen.GetHostImageObject(0));
        }
    }
}
