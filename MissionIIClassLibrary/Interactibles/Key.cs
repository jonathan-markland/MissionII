using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Interactibles
{
    public class Key : InteractibleObject
    {
        public Key(int roomNumber)
            : base(new SpriteInstance { Traits = MissionIISprites.Key }, roomNumber)
        {
        }

        public override int CollectionScore => Constants.KeyCollectionScore;
    }
}
