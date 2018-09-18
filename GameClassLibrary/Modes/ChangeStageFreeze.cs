
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
        private int _countDown;
        private bool _firstCycle = true;
        private readonly GameMode _previousMode;
        private readonly Func<GameMode> _getNextModeFunction;
        private readonly Sound.SoundTraits _freezeSound;



        public ChangeStageFreeze(
            int freezeCycles, 
            GameMode previousMode,
            Sound.SoundTraits optionalFreezeSound,
            Func<GameMode> getNextModeFunction)
        {
            _freezeSound = optionalFreezeSound;
            _countDown = freezeCycles;
            _previousMode = previousMode;
            _getNextModeFunction = getNextModeFunction;
        }



        public override void AdvanceOneCycle(KeyStates theKeyStates)
        {
            if (_firstCycle)
            {
                _freezeSound?.Play();
                _firstCycle = false;
            }

            if (_countDown > 0)
            {
                --_countDown;
            }
            else
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
