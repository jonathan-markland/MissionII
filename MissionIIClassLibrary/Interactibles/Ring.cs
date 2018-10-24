using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.GameBoard;

namespace MissionIIClassLibrary.Interactibles
{
    public class Ring : MissionIIInteractibleObject
    {
        public Ring(int roomNumber, Action<InteractibleObject, int> collectObject) 
            : base(new SpriteInstance { Traits = MissionIISprites.Ring }, roomNumber, collectObject)
        {
        }

        public override int CollectionScore => Constants.RingCollectionScore;
    }
}
