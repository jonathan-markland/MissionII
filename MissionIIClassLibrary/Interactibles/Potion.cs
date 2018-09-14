using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Interactibles
{
    public class Potion : MissionIIInteractibleObject
    {
        public Potion(int roomNumber)
            : base(new SpriteInstance { Traits = MissionIISprites.Potion }, roomNumber)
        {
        }

        public override void ManWalkedIntoYou(IGameBoard theGameBoard)
        {
            theGameBoard.PlayerGainLife();
            theGameBoard.Remove(this);
        }

        public override int CollectionScore => 0;
    }
}
