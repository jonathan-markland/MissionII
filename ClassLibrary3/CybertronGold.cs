
namespace GameClassLibrary
{
    public class CybertronGold : CybertronObject
    {
        public CybertronGold(int roomNumber) 
            : base(new SpriteInstance { Traits = CybertronSpriteTraits.Gold }, roomNumber)
        {
        }
    }
}
