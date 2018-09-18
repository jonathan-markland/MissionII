
using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace GameClassLibrary.Modes
{
    /// <summary>
    /// Freeze the display of the current mode for a period, then
    /// change mode.  A sound is played to accompany the freeze.
    /// </summary>
    public class ChangeStageFreeze : GameMode
    {
        private readonly uint _freezeCycles;
        private readonly Time.CycleSnapshot _startTime;
        private readonly GameMode _previousMode;
        private readonly Func<GameMode> _getNextModeFunction;



        public ChangeStageFreeze(
            uint freezeCycles, 
            GameMode previousMode,
            Sound.SoundTraits optionalFreezeSound,
            Func<GameMode> getNextModeFunction)
        {
            _freezeCycles = freezeCycles;
            _previousMode = previousMode;
            _getNextModeFunction = getNextModeFunction;
            _startTime = Time.CycleSnapshot.Now;

            optionalFreezeSound?.Play(); // TODO: No, do in AdvanceOneCycle()
        }



        public override void AdvanceOneCycle(KeyStates theKeyStates)
        {
            if (_startTime.HasElapsed(_freezeCycles))
            {
                ActiveMode = _getNextModeFunction();
            }
        }



        public override void Draw(IDrawingTarget drawingTarget)
        {
            _previousMode.Draw(drawingTarget);
        }
    }
}
