using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace GameClassLibrary.Modes
{
    public struct ModeFunctions
    {
        public Action<KeyStates> AdvanceOneCycle { get; private set; }
        public Action<IDrawingTarget> Draw { get; private set; }

        public ModeFunctions(
            Action<KeyStates> advanceOneCycle,
            Action<IDrawingTarget> draw)
        {
            AdvanceOneCycle = advanceOneCycle;
            Draw = draw;
        }
    }
}
