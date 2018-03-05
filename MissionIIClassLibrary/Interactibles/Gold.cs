﻿using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Interactibles
{
    public class Gold : InteractibleObject
    {
        public Gold(int roomNumber) 
            : base(new SpriteInstance { Traits = MissionIISprites.Gold }, roomNumber)
        {
        }

        public override int CollectionScore => Constants.GoldCollectionScore;
    }
}
