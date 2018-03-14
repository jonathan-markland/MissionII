using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Modes
{
    public class HiScoreMode : BaseGameMode
    {
        private int _countDown = Constants.TitleScreenRollCycles;
        private bool _enterScoreMode;
        private GameClassLibrary.Hiscore.HiScoreScreenControl _hiScoreScreenControl;

        /// <summary>
        /// Constructor for just showing the hi-score screen.
        /// </summary>
        public HiScoreMode()
        {
            CreateHiScoreControl();
            _enterScoreMode = false;
        }

        /// <summary>
        /// Constructor for finishing a game.
        /// </summary>
        public HiScoreMode(uint scoreAchieved)
        {
            CreateHiScoreControl();
            if (_hiScoreScreenControl.CanPlayerEnterTable(scoreAchieved))
            {
                _enterScoreMode = true;
                _hiScoreScreenControl.ForceEnterScore(scoreAchieved);
            }
            else
            {
                _enterScoreMode = false;
            }
        }

        private void CreateHiScoreControl()
        {
            _hiScoreScreenControl = new GameClassLibrary.Hiscore.HiScoreScreenControl(
                new GameClassLibrary.Hiscore.HiScoreScreenDimensions
                { TopEdgeY = 70, BottomEdgeY = 246, NamesLeftX = 10, ScoresRightX = 310 },  // TODO: screen dimension constants!
                MissionIISprites.NarrowFont,
                MissionIISprites.Life,
                MissionIIGameBoard.HiScoreTableModel);
        }

        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
        {
            if (_enterScoreMode)
            {
                if (_hiScoreScreenControl.InEditMode)
                {
                    var hsKeyStates = new GameClassLibrary.Hiscore.HiScoreTableKeyStates();
                    hsKeyStates.Down = theKeyStates.Down;
                    hsKeyStates.Up = theKeyStates.Up;
                    hsKeyStates.Left = theKeyStates.Left;
                    hsKeyStates.Fire = theKeyStates.Fire;
                    _hiScoreScreenControl.AdvanceOneCycle(hsKeyStates);
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
            _hiScoreScreenControl.DrawScreen(drawingTarget);
        }
    }
}
