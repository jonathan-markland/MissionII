using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.ArtificialIntelligence
{
    public class Swoop : AbstractIntelligenceProvider
    {
        public override void AdvanceOneCycle(IGameBoard theGameBoard, SpriteInstance spriteInstance)
        {
            for (int i = 0; i < Constants.GhostMovementCycles; i++)
            {
                var moveDeltas = Business.GetMovementDeltasToHeadTowards(
                    spriteInstance,
                    theGameBoard.ManSpriteInstance());

                spriteInstance.MoveBy(moveDeltas);

                if (spriteInstance.Intersects(theGameBoard.ManSpriteInstance()))
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
