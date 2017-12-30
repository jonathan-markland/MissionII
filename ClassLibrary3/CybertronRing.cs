
namespace GameClassLibrary
{
    public class CybertronRing : CybertronObject
    {
        public CybertronRing(int roomNumber) 
            : base(new SpriteInstance { Traits = CybertronSpriteTraits.Ring }, roomNumber)
        {
        }
    }
}
