
using GameClassLibrary.Math;
using GameClassLibrary.GameBoard;

namespace GameClassLibrary.ArtificialIntelligence
{
    public class Attractor : AbstractIntelligenceProvider
    {
        private bool _operationEnable = false;



        public override void AdvanceOneCycle(IGameBoard theGameBoard, GameObject gameObject)
        {
            _operationEnable = !_operationEnable;  // ie: operate only ever other cycle
            if (_operationEnable)
            {
                var moveDeltas = gameObject.GetMovementDeltasToHeadTowards(
                    theGameBoard.GetMan());

                // We must separate horizontal and vertical movement in order to avoid
                // things getting 'stuck' on walls because they can't move horizontally
                // into the wall, but can moe vertically downward.  Trying to do both
                // directions at once results in rejection of the move, and the
                // sticking problem.

                theGameBoard.MoveAdversaryOnePixel(
                    gameObject,
                    new MovementDeltas(moveDeltas.dx, 0));

                theGameBoard.MoveAdversaryOnePixel(
                    gameObject,
                    new MovementDeltas(0, moveDeltas.dy));
            }
        }
    }
}
