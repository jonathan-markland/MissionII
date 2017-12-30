
namespace GameClassLibrary
{
    public class CybertronKey : CybertronObject
    {
        public CybertronKey(int roomNumber) 
            : base(new SpriteInstance { Traits = CybertronSpriteTraits.Key }, roomNumber)
        {
        }
    }
}
