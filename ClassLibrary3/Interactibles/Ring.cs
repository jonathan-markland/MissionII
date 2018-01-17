using GameClassLibrary.Math;

namespace GameClassLibrary.Interactibles
{
    public class Ring : InteractibleObject
    {
        public Ring(int roomNumber) 
            : base(new SpriteInstance { Traits = CybertronSpriteTraits.Ring }, roomNumber)
        {
        }
    }
}
