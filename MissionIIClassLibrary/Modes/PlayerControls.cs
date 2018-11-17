
using GameClassLibrary.Modes;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Modes
{
    public static class PlayerControls
    {
        public static ModeFunctions New()
        {
            int countDown = (Constants.TitleScreenRollCycles * 3) / 2;

            return new ModeFunctions(

                // -- Advance one cycle --

                keyStates =>
                {
                    if (keyStates.Fire)
                    {
                        GameMode.ActiveMode = StartNewGame.New();
                    }
                    else if (countDown == 0)
                    {
                        GameMode.ActiveMode = MissionRotatingInstructions.New();
                    }
                    else
                    {
                        --countDown;
                    }
                },

                // -- Draw --

                drawingTarget =>
                {
                    var cx = Screen.Width / 2;

                    drawingTarget.ClearScreen();
                    drawingTarget.DrawSprite(0, 0, MissionIISprites.Background.GetHostImageObject(0));
                    drawingTarget.DrawText(cx, 50, "CONTROLS", MissionIIFonts.GiantFont, TextAlignment.Centre);

                    int idx = (int)(GameClassLibrary.Time.CycleCounter.Count32 / 16) & 1;

                    var keysW = MissionIISprites.CursorKeys.Width;
                    var keysH = MissionIISprites.CursorKeys.Height;
                    var manW = MissionIISprites.WalkingLeft.Width;  // widest
                    var manH = MissionIISprites.WalkingUp.Height;   // tallest

                    var manOffset = 10 + ((int)((GameClassLibrary.Time.CycleCounter.Count32 / 4) & 31));
                    var x = 70;
                    var y = 170;
                    var dx = (keysW + manW + manOffset) / 2;
                    var dy = (keysH + manH + manOffset) / 2;
                    drawingTarget.DrawFirstSpriteCentred(x, y, MissionIISprites.CursorKeys);
                    drawingTarget.DrawSpriteCentred(x - dx, y, MissionIISprites.WalkingLeft, idx);
                    drawingTarget.DrawSpriteCentred(x + dx, y, MissionIISprites.WalkingRight, idx);
                    drawingTarget.DrawSpriteCentred(x, y - dy, MissionIISprites.WalkingUp, idx);
                    drawingTarget.DrawSpriteCentred(x, y + dy, MissionIISprites.WalkingDown, idx);

                    drawingTarget.DrawText(x, 220, "OR JOYSTICK", MissionIIFonts.NarrowFont, TextAlignment.Centre);

                    x = 160;

                    drawingTarget.DrawFirstSprite(x, 140, MissionIISprites.ZKey);
                    drawingTarget.DrawText(x+30, 145, "FIRE  AND  START", MissionIIFonts.NarrowFont, TextAlignment.Left);

                    drawingTarget.DrawFirstSprite(x, 190, MissionIISprites.PKey);
                    drawingTarget.DrawText(x + 30, 195, "PAUSE", MissionIIFonts.NarrowFont, TextAlignment.Left);
                    drawingTarget.DrawText(x + 30, 205, "AND LEVEL SELECT", MissionIIFonts.NarrowFont, TextAlignment.Left);
                });

        }
    }
}
