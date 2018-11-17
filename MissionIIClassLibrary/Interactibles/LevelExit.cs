
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Interactibles
{
    public class LevelExit : MissionIIInteractibleObject
    {
        public LevelExit(int roomNumber) 
            : base(new SpriteInstance { Traits = MissionIISprites.LevelExit }, roomNumber)
        {
        }
    }
}
