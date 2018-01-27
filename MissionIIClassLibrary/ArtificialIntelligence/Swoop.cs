﻿
namespace MissionIIClassLibrary.ArtificialIntelligence
{
    public class Swoop : AbstractIntelligenceProvider
    {
        public override void AdvanceOneCycle(MissionIIGameBoard theGameBoard, SpriteInstance spriteInstance)
        {
            for (int i = 0; i < Constants.GhostMovementCycles; i++)
            {
                var moveDeltas = Business.GetMovementDeltasToHeadTowards(
                    spriteInstance,
                    theGameBoard.Man.SpriteInstance);

                spriteInstance.MoveBy(moveDeltas);

                if (spriteInstance.Intersects(theGameBoard.Man.SpriteInstance))
                {
                    KillMan(theGameBoard);
                }
            }
        }

        

        private void KillMan(MissionIIGameBoard theGameBoard)
        {
            theGameBoard.Man.Electrocute(ElectrocutionMethod.ByDroid);
        }

    }
}
