
using System;
using GameClassLibrary.Input;

namespace GameClassLibrary.GameBoard
{
    public abstract class InteractibleObject : GameObject
    {
        public InteractibleObject()
        {
        }



        public override void AdvanceOneCycle(KeyStates theKeyStates)
        {
            // No action required.
        }



        public override ShotStruct YouHaveBeenShot(bool shotByMan)
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
