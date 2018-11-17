using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.GameBoard;

namespace MissionIIClassLibrary.Interactibles
{
    public class Key : MissionIIInteractibleObject
    {
        public Key(int roomNumber)
            : base(new SpriteInstance { Traits = MissionIISprites.Key }, roomNumber)
        {
        }
    }
}
