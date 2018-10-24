using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.GameBoard;

namespace MissionIIClassLibrary.Interactibles
{
    public class Gold : MissionIIInteractibleObject
    {
        public Gold(int roomNumber, Action<InteractibleObject, int> collectObject) 
            : base(new SpriteInstance { Traits = MissionIISprites.Gold }, roomNumber, collectObject)
        {
        }

        public override int CollectionScore => Constants.GoldCollectionScore;
    }
}
