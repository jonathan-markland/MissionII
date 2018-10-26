using GameClassLibrary.Graphics;
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public static class EnteringLevel
    {
        public static ModeFunctions New(
            MissionIIGameBoard gameBoard)
        {
            int countDown = Constants.EnteringLevelScreenCycles;
            var levelAccessCode = string.Empty;

            if (LevelHasAccessCode(gameBoard.GetLevelNumber()))
            {
                levelAccessCode = GameClassLibrary.Algorithms.LevelAccessCodes.GetForLevel(gameBoard.GetLevelNumber());
            }

            var thisModeFuncs = new ModeFunctions();

            thisModeFuncs.SetAfterwards(

                // -- Advance one cycle --

                keyStates =>
                {
                    if (MissionIIModes.HandlePause(gameBoard, keyStates, thisModeFuncs)) return;
                    if (countDown == Constants.EnteringLevelScreenCycles)
                    {
                        GameClassLibrary.Sound.SoundTraits.StopBackgroundMusic();
                        MissionIISounds.EnteringLevel.Play();
                    }
                    if (countDown > 0)
                    {
                        --countDown;
                    }
                    else
                    {
                        GameMode.ActiveMode = GamePlay.New(gameBoard);
                    }
                },

                // -- Draw --

                drawingTarget =>
                {
                    drawingTarget.ClearScreen();
                    drawingTarget.DrawSprite(0, 0, MissionIISprites.Background.GetHostImageObject(0));

                    var cx = Constants.ScreenWidth / 2;
                    var levelNumber = gameBoard.GetLevelNumber();

                    drawingTarget.DrawText(cx, 40, "LEVEL " + levelNumber, MissionIIFonts.GiantFont, TextAlignment.Centre);

                    if (LevelHasAccessCode(levelNumber))
                    {
                        drawingTarget.DrawText(Constants.ScreenWidth - 10, 50, "ACCESS", MissionIIFonts.NarrowFont, TextAlignment.Right);
                        drawingTarget.DrawText(Constants.ScreenWidth - 10, 80, levelAccessCode, MissionIIFonts.NarrowFont, TextAlignment.Right);
                    }

                    drawingTarget.DrawText(cx, 110, "FIND THE FOLLOWING ITEMS", MissionIIFonts.NarrowFont, TextAlignment.Centre);

                    // Show the things you need to find on this level.

                    int x = Constants.ScreenWidth / 2;
                    int y = 130; // TODO: constant
                    int dy = 24; // TODO: constant

                    gameBoard.ForEachThingWeHaveToFindOnThisLevel(
                        o =>
                        {
                            drawingTarget.DrawFirstSpriteCentred(x, y, ((Interactibles.MissionIIInteractibleObject)o).SpriteTraits);  // TODO:  Ideally use o.Draw
                            y += dy;
                        });

                    drawingTarget.DrawText(cx, 210, "THEN FIND THE LEVEL EXIT", MissionIIFonts.NarrowFont, TextAlignment.Centre);
                    drawingTarget.DrawFirstSpriteCentred(x, 230, MissionIISprites.LevelExit);
                });

            return thisModeFuncs;
        }



        private static bool LevelHasAccessCode(int levelNumber)
        {
            return (levelNumber >= Constants.FirstLevelWithAccessCode && levelNumber <= Constants.LastLevelWithAccessCode);
        }
    }
}
