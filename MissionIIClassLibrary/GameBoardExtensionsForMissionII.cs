
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
    }
}
