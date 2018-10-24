using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.GameBoard;

namespace MissionIIClassLibrary.Interactibles
{
    public class LevelExit : MissionIIInteractibleObject
    {
        private readonly Action _seeIflevelObjectivesMet;

        // TODO: This should not need to be passed the collectObject function!
        public LevelExit(int roomNumber, Action<InteractibleObject, int> collectObject, Action seeIflevelObjectivesMet) 
            : base(new SpriteInstance { Traits = MissionIISprites.LevelExit }, roomNumber, collectObject)
        {
            _seeIflevelObjectivesMet = seeIflevelObjectivesMet;
        }

        public override int CollectionScore => 0;

        public override void ManWalkedIntoYou()
        {
            _seeIflevelObjectivesMet();
        }
    }
}
