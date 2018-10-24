using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.GameBoard;

namespace MissionIIClassLibrary.Interactibles
{
    public class LevelExit : MissionIIInteractibleObject
    {
        private readonly Func<bool> _levelObjectivesMet;

        // TODO: This should not need to be passed the collectObject function!
        public LevelExit(int roomNumber, Action<InteractibleObject, int> collectObject, Func<bool> levelObjectivesMet) 
            : base(new SpriteInstance { Traits = MissionIISprites.LevelExit }, roomNumber, collectObject)
        {
            _levelObjectivesMet = levelObjectivesMet;
        }

        public override int CollectionScore => 0;

        public override void ManWalkedIntoYou(IGameBoard theGameBoard)
        {
            if (_levelObjectivesMet())
            {
                GameClassLibrary.Modes.GameMode.ActiveMode = new Modes.LeavingLevel(theGameBoard);
            }
        }
    }
}
