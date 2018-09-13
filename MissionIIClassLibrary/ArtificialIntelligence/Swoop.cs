using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.ArtificialIntelligence
{
    public class Swoop : AbstractIntelligenceProvider
    {
        public override void AdvanceOneCycle(IGameBoard theGameBoard, GameObject gameObject)
        {
            for (int i = 0; i < Constants.GhostMovementCycles; i++)
            {
                var moveDeltas = gameObject.GetMovementDeltasToHeadTowards(
                    theGameBoard.GetMan());

                gameObject.MoveBy(moveDeltas);

                if (gameObject.Intersects(theGameBoard.GetMan()))
                {
                    KillMan(theGameBoard);
                }
            }
        }

        

        private void KillMan(IGameBoard theGameBoard)
        {
            theGameBoard.Electrocute(ElectrocutionMethod.ByDroid);
        }

    }
}
