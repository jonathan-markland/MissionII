
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public static class GamePlay
    {
        public static ModeFunctions New(
            MissionIIGameBoard gameBoard)
        {
            var thisFuncs = new ModeFunctions();

            thisFuncs.SetAfterwards(

                // -- Advance one cycle --

                keyStates =>
                {
                    if (MissionIIModes.HandlePause(gameBoard, keyStates, thisFuncs)) return;
                    gameBoard.AdvanceOneCycle(keyStates); // TODO: pull logic into this class
                },

                // -- Draw --

                drawingTarget =>
                {
                    gameBoard.DrawBoardToTarget(drawingTarget);
                });

            return thisFuncs;
        }
    }
}
