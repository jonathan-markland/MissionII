
namespace GameClassLibrary.ArtificialIntelligence
{
    public class Swoop : AbstractIntelligenceProvider
    {
        public override void AdvanceOneCycle(CybertronGameBoard theGameBoard, SpriteInstance spriteInstance)
        {
            for (int i = 0; i < Constants.GhostMovementCycles; i++)
            {
                var moveDeltas = CybertronGameStateUpdater.GetMovementDeltasToHeadTowards(
                    spriteInstance,
                    theGameBoard.Man.SpriteInstance);

                CybertronGameStateUpdater.MoveAdversaryOnePixelUnchecked(
                    spriteInstance,
                    moveDeltas);

                if (spriteInstance.Intersects(theGameBoard.Man.SpriteInstance))
                {
                    KillMan(theGameBoard);
                }
            }
        }

        

        private void KillMan(CybertronGameBoard theGameBoard)
        {
            if (!theGameBoard.Man.IsDead)
            {
                theGameBoard.Man.Die(); // only make the call once per room.
            }
        }

    }
}
