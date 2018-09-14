using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Interactibles
{
    public class Gold : MissionIIInteractibleObject
    {
        public Gold(int roomNumber) 
            : base(new SpriteInstance { Traits = MissionIISprites.Gold }, roomNumber)
        {
        }

        public override int CollectionScore => Constants.GoldCollectionScore;
    }
}
