
using System;
using GameClassLibrary.Math;
using GameClassLibrary.GameBoard;

namespace GameClassLibrary.ArtificialIntelligence
{
    public static class Swoop
    {
        public static ArtificialIntelligenceFunctions NewSwoop(
            GameObject gameObject,
            Action manDestroyAction,
            Func<Rectangle> getManExtents)
        {
            return new ArtificialIntelligenceFunctions(() => 
            {
                for (int i = 0; i < Constants.SwoopMovementCycles; i++)
                {
                    var moveDeltas = gameObject.GetBoundingRectangle().GetMovementDeltasToHeadTowards(getManExtents());

                    gameObject.MoveBy(moveDeltas);

                    if (gameObject.GetBoundingRectangle().Intersects(getManExtents()))
                    {
                        manDestroyAction();
                    }
                }
            });
        }
    }
}
