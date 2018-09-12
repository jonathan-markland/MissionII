using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Interactibles
{
    public class Potion : InteractibleObject
    {
        public Potion(int roomNumber)
            : base(new SpriteInstance { Traits = MissionIISprites.Potion }, roomNumber)
        {
        }

        public override void ManWalkedIntoYou(IGameBoard theGameBoard)
        {
            theGameBoard.PlayerGainLife();
            RemoveThisObject(theGameBoard);
        }

        public override int CollectionScore => 0;
    }
}
