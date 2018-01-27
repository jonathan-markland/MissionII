
namespace MissionIIClassLibrary.Interactibles
{
    public class Ring : InteractibleObject
    {
        public Ring(int roomNumber) 
            : base(new SpriteInstance { Traits = CybertronSpriteTraits.Ring }, roomNumber)
        {
        }

        public override int CollectionScore => CybertronGameBoardConstants.RingCollectionScore;
    }
}
