
namespace GameClassLibrary.Interactibles
{
    public class Key : InteractibleObject
    {
        public Key(int roomNumber) 
            : base(new SpriteInstance { Traits = CybertronSpriteTraits.Key }, roomNumber)
        {
        }
    }
}
