
namespace MissionIIClassLibrary.Interactibles
{
    public class Ring : InteractibleObject
    {
        public Ring(int roomNumber) 
            : base(new SpriteInstance { Traits = MissionIISpriteTraits.Ring }, roomNumber)
        {
        }

        public override int CollectionScore => MissionIIGameBoardConstants.RingCollectionScore;
    }
}
