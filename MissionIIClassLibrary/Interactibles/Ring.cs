using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Interactibles
{
    public class Ring : MissionIIInteractibleObject
    {
        public Ring(int roomNumber) 
            : base(new SpriteInstance { Traits = MissionIISprites.Ring }, roomNumber)
        {
        }

        public override int CollectionScore => Constants.RingCollectionScore;
    }
}
