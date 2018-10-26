using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace GameClassLibrary.Modes
{
    public class ModeFunctions
    {
        public Action<KeyStates> AdvanceOneCycle { get; private set; }
        public Action<IDrawingTarget> Draw { get; private set; }

        public ModeFunctions() { }

        public ModeFunctions(
            Action<KeyStates> advanceOneCycle,
            Action<IDrawingTarget> draw)
        {
            AdvanceOneCycle = advanceOneCycle;
            Draw = draw;
        }

        public void SetAfterwards(
            Action<KeyStates> advanceOneCycle,
            Action<IDrawingTarget> draw)
        {
            AdvanceOneCycle = advanceOneCycle;
            Draw = draw;
        }
    }
}
