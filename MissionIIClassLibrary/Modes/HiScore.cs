using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Modes
{
    public class HiScore : BaseGameMode
    {
        private static GameClassLibrary.Hiscore.HiScoreScreenModel HiScoreTableModel;
        private int _countDown = Constants.TitleScreenRollCycles;
        private bool _enterScoreMode;
        private bool _justEnteredName;
        private GameClassLibrary.Hiscore.HiScoreScreenControl _hiScoreScreenControl;

        /// <summary>
        /// Construction must be called once on program start up.
        /// </summary>
        public static void StaticInit()
        {
            HiScoreTableModel =
                new GameClassLibrary.Hiscore.HiScoreScreenModel(
                    Constants.InitialLowestHiScore,
                    Constants.InitialHiScoresIncrement);
        }

        /// <summary>
        /// Constructor for just showing the hi-score screen.
        /// </summary>
        public HiScore()
        {
            CreateHiScoreControl();
            _enterScoreMode = false;
            _justEnteredName = false;
        }

        /// <summary>
        /// Constructor for finishing a game.
        /// </summary>
        public HiScore(uint scoreAchieved)
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
                new GameClassLibrary.Math.Rectangle(10, 70, 300, 246-70),// TODO: screen dimension constants!
                MissionIIFonts.NarrowFont,
                MissionIISprites.Life,
                HiScoreTableModel);
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
                    _justEnteredName = true;
                }
            }
            else // show
            {
                if(_countDown > 0)
                {
                    if (theKeyStates.Fire)
                    {
                        if (!_justEnteredName || _countDown < (Constants.TitleScreenRollCycles * 3 / 4))
                        {
                            MissionIIGameModeSelector.ModeSelector.CurrentMode = new StartNewGame();
                        }
                    }
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
            if (_enterScoreMode)
            {
                drawingTarget.DrawText(Constants.ScreenWidth / 2, 56, "ENTER YOUR NAME", MissionIIFonts.WideFont, TextAlignment.Centre);
            }
        }
    }
}
