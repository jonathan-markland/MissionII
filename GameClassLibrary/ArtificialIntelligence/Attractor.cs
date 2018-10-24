
using System;
using GameClassLibrary.GameBoard;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;

namespace GameClassLibrary.ArtificialIntelligence
{
    public class Attractor : AbstractIntelligenceProvider
    {
        private readonly Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> _moveAdversaryOnePixel;
        private readonly Func<Rectangle> _getManExtents;

        public Attractor(
            Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel,
            Func<Rectangle> getManExtents)
        {
            _moveAdversaryOnePixel = moveAdversaryOnePixel;
            _getManExtents = getManExtents;
        }

        public override void AdvanceOneCycle(GameObject gameObject)
        {
            if ((Time.CycleCounter.Count32 & 1) == 0)
            {
                var moveDeltas = gameObject.GetBoundingRectangle().GetMovementDeltasToHeadTowards(_getManExtents());

                // We must separate horizontal and vertical movement in order to avoid
                // things getting 'stuck' on walls because they can't move horizontally
                // into the wall, but can moe vertically downward.  Trying to do both
                // directions at once results in rejection of the move, and the
                // sticking problem.

                _moveAdversaryOnePixel(gameObject, moveDeltas.XComponent);
                _moveAdversaryOnePixel(gameObject, moveDeltas.YComponent);  // TODO: Resolve these within the function (one call only).
            }
        }
    }
}
