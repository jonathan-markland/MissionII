
using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.Controls.Hiscore;

namespace GameClassLibrary.Modes
{
    public static class HiScoreEntry
    {
        public static HiScoreScreenModel HiScoreTableModel;  // TODO: reconsider



        /// <summary>
        /// Construction must be called once on program start up.
        /// </summary>
        public static void StaticInit(uint initialLowestHiScore, uint initialHiScoresIncrement)
        {
            HiScoreTableModel =
                new HiScoreScreenModel(
                    initialLowestHiScore,
                    initialHiScoresIncrement);
        }



        public static ModeFunctions New(
            uint screenCycles,
            SpriteTraits backgroundSprite,
            Font titleFont,
            Font enterNameFont,
            Font tableFont,
            SpriteTraits cursorSprite,
            uint scoreAchieved,
            Func<ModeFunctions> getTitleScreenModeFunction,
            Func<ModeFunctions> getStartNewGameModeFunction,
            Func<ModeFunctions> getRollOverModeFunction)
        {
            var hiScoreScreenControl = new HiScoreScreenControl(
                new GameClassLibrary.Math.Rectangle(10, 70, 300, 246 - 70),// TODO: screen dimension constants!
                tableFont,
                cursorSprite,
                HiScoreTableModel);

            hiScoreScreenControl.ForceEnterScore(scoreAchieved);

            return new ModeFunctions(

                // -- Advance one cycle --

                keyStates =>
                {
                    if (hiScoreScreenControl.InEditMode)
                    {
                        hiScoreScreenControl.AdvanceOneCycle(keyStates);
                    }
                    else
                    {
                        var hiScoreShow = HiScoreShow.New(
                                screenCycles, backgroundSprite, titleFont, tableFont,
                                getStartNewGameModeFunction, getTitleScreenModeFunction);

                        GameMode.ActiveMode = ChangeStageFreeze.New(
                            screenCycles,
                            hiScoreShow,
                            null,
                            getTitleScreenModeFunction);
                    }
                },

                // -- Draw --

                drawingTarget =>
                {
                    drawingTarget.ClearScreen();
                    drawingTarget.DrawSprite(0, 0, backgroundSprite.GetHostImageObject(0));
                    drawingTarget.DrawText(Screen.Width / 2, 10, "HI SCORES", titleFont, TextAlignment.Centre);
                    hiScoreScreenControl.DrawScreen(drawingTarget);
                    drawingTarget.DrawText(
                        Screen.Width / 2, 56, "ENTER YOUR NAME", enterNameFont, TextAlignment.Centre);
                });
        }
    }
}
