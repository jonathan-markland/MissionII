
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
    }
}
