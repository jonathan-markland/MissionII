using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Modes
{
    public class RotatingInstructions : BaseGameMode
    {
        private const int ScreenCount = 3;
        private int _countDown = Constants.TitleScreenRollCycles * ScreenCount;

        public override void AdvanceOneCycle(MissionIIKeyStates theKeyStates)
        {
            if (theKeyStates.Fire)
            {
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new StartNewGame();
            }
            else if (_countDown == 0)
            {
                MissionIIGameModeSelector.ModeSelector.CurrentMode = new TitleScreen();
            }
            else
            {
                --_countDown;
            }
        }

        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawSprite(0, 0, MissionIISprites.Background.GetHostImageObject(0));
            var theFont = MissionIIFonts.NarrowFont;
            var cx = Constants.ScreenWidth / 2;
            var c = TextAlignment.Centre;
            if (_countDown < Constants.TitleScreenRollCycles * 1)
            {
                drawingTarget.DrawText(cx,  60, "VERSION BY JONATHAN MARKLAND", theFont, c);
                drawingTarget.DrawText(cx, 100, "BASED ON AN", theFont, c);
                drawingTarget.DrawText(cx, 120, "ORIGINAL CONCEPT", theFont, c);
                drawingTarget.DrawText(cx, 140, "BY MATTHEW BATES", theFont, c);
                drawingTarget.DrawText(cx, 180, "SOUNDS FROM FREESOUND WEBSITE", theFont, c);
            }
            else if (_countDown < Constants.TitleScreenRollCycles * 2)
            {
                drawingTarget.DrawText(cx,  50, "MOVE USING CURSOR KEYS", theFont, c);
                drawingTarget.DrawText(cx,  70, "Z      FIRE", theFont, c);
                drawingTarget.DrawText(cx,  90, "P      PAUSE", theFont, c);
                drawingTarget.DrawText(cx, 110, "OR JOYSTICK OR PAD", theFont, c);
                drawingTarget.DrawText(cx, 160, "F11 F12   FULL SCREEN TOGGLE", theFont, c);
                drawingTarget.DrawText(cx, 180, "F2 F3     VIEW SIZE", theFont, c);
            }
            else if (_countDown < Constants.TitleScreenRollCycles * 3)
            {
                drawingTarget.DrawText(cx, 100, "COLLECT OBJECTS ON LEVEL", theFont, c);
                drawingTarget.DrawText(cx, 120, "THEN FIND THE SAFE", theFont, c);
                drawingTarget.DrawText(cx, 160, "AVOID ELECTROCUTION", theFont, c);
            }
        }
    }
}
