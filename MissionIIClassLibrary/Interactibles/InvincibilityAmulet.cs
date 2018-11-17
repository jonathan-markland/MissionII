
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Interactibles
{
    public class InvincibilityAmulet : MissionIIInteractibleObject
    {
        public InvincibilityAmulet(int roomNumber)
            : base(new SpriteInstance { Traits = MissionIISprites.InvincibilityAmulet }, roomNumber)
        {
        }
    }
}
