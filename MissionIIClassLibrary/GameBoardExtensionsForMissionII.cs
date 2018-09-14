
using GameClassLibrary.Math;
using GameClassLibrary.GameBoard;

namespace MissionIIClassLibrary
{
    public static class GameBoardExtensionsForMissionII
    {
        /// <summary>
        /// Returns true if any droids exist in the room.
        /// </summary>
        public static bool DroidsExistInRoom(this IGameBoard gameBoard)
        {
            bool foundDroids = false;
            gameBoard.ForEachObjectInPlayDo<Droids.BaseDroid>(o =>
            {
                foundDroids = true;   // TODO: Library issue:  It's not optimal that we can't break the ForEach.
            });
            return foundDroids;
        }



        public static GameClassLibrary.Math.Point GetCornerFurthestAwayFromMan(this IGameBoard gameBoard)
        {
            var cx = Constants.ScreenWidth / 2;
            var cy = Constants.ScreenHeight / 2;
            var manCentre = gameBoard.GetMan().GetBoundingRectangle().Centre;
            var x = manCentre.X < cx ? Constants.ScreenWidth : 0;
            var y = manCentre.Y < cy ? Constants.ScreenHeight : 0;
            return new Point(x, y);
        }



    }
}
