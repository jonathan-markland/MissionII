
namespace GameClassLibrary.Interactibles
{
    public class CybertronGold : CybertronObject
    {
        public CybertronGold(int roomNumber) 
            : base(new SpriteInstance { Traits = CybertronSpriteTraits.Gold }, roomNumber)
        {
        }
    }
}
