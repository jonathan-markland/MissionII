using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.GameBoard;

namespace MissionIIClassLibrary.Interactibles
{
    public class Gold : MissionIIInteractibleObject
    {
        public Gold(int roomNumber) 
            : base(new SpriteInstance { Traits = MissionIISprites.Gold }, roomNumber)
        {
        }
    }
}
