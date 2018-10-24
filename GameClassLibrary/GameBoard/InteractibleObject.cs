
using System;
using GameClassLibrary.Input;

namespace GameClassLibrary.GameBoard
{
    public abstract class InteractibleObject : GameObject
    {
        private readonly Action<InteractibleObject, int> _collectObject;


        public InteractibleObject(Action<InteractibleObject, int> collectObject)
        {
            _collectObject = collectObject;
        }



        public override void AdvanceOneCycle(IGameBoard theGameBoard, KeyStates theKeyStates)
        {
            // No action required.
        }



        public override void ManWalkedIntoYou(IGameBoard theGameBoard)
        {
            _collectObject(this, CollectionScore);
        }



        public override ShotStruct YouHaveBeenShot(IGameBoard theGameBoard, bool shotByMan)
        {
            // This cannot be shot (ignore)
			return new ShotStruct(affirmed: false, scoreIncrease:0);
        }



        public override bool CanBeOverlapped
        {
            get { return true; }
        }
    }
}
