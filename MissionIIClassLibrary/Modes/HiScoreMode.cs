using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Modes
{
    public class HiScoreMode : BaseGameMode
    {
        private int _countDown = Constants.TitleScreenRollCycles;
        private bool _enterScoreMode;

        /// <summary>
        /// Constructor for just showing the hi-score screen.
        /// </summary>
        public HiScoreMode()
        {
            _enterScoreMode = false;
        }

        /// <summary>
        /// Constructor for finishing a game.
        /// </summary>
        public HiScoreMode(uint scoreAchieved)
        {
            if (MissionIIGameBoard.HiScoreTable.CanPlayerEnterTable(scoreAchieved))
            {
                _enterScoreMode = true;
                MissionIIGameBoard.HiScoreTable.ForceEnterScore(scoreAchieved);
            }
            else
            {
                _enterScoreMode = false;
            }
        }

        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
        {
            if (_enterScoreMode)
            {
                if (MissionIIGameBoard.HiScoreTable.InEditMode)
                {
                    var hsKeyStates = new GameClassLibrary.Hiscore.HiScoreTableKeyStates();
                    hsKeyStates.Down = theKeyStates.Down;
                    hsKeyStates.Up = theKeyStates.Up;
                    hsKeyStates.Left = theKeyStates.Left;
                    hsKeyStates.Fire = theKeyStates.Fire;
                    MissionIIGameBoard.HiScoreTable.AdvanceOneCycle(hsKeyStates);
                }
                else
                {
                    _enterScoreMode = false;
                }
            }
            else // show
            {
                if (theKeyStates.Fire)
                {
                    MissionIIGameModeSelector.ModeSelector.CurrentMode = new TitleScreen();
                }
                else if(_countDown > 0)
                {
                    --_countDown;
                }
                else
                {
                    MissionIIGameModeSelector.ModeSelector.CurrentMode = new RotatingInstructions();
                }
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawSprite(0, 0, MissionIISprites.HiScoreScreen.GetHostImageObject(0));
            MissionIIGameBoard.HiScoreTable.DrawScreen(drawingTarget);
        }
    }
}
