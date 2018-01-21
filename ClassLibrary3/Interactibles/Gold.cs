﻿
namespace GameClassLibrary.Interactibles
{
    public class Gold : InteractibleObject
    {
        public Gold(int roomNumber) 
            : base(new SpriteInstance { Traits = CybertronSpriteTraits.Gold }, roomNumber)
        {
        }
    }
}