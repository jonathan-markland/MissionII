using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.GameBoard;

namespace MissionIIClassLibrary.Interactibles
{
    public class Key : MissionIIInteractibleObject
    {
        public Key(int roomNumber, Action<InteractibleObject, int> collectObject)
            : base(new SpriteInstance { Traits = MissionIISprites.Key }, roomNumber, collectObject)
        {
        }

        public override int CollectionScore => Constants.KeyCollectionScore;
    }
}
