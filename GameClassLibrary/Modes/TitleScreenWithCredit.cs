
using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace GameClassLibrary.Modes
{
    public class TitleScreenWithCredit : GameMode
    {
        private readonly int _titleScreenRollCycles;
        private readonly int _fireButtonPressEnableTime;
        private readonly Sound.SoundTraits _introSound;
        private readonly SpriteTraits _titleScreenBackground;
        private readonly Font _font;
        private readonly Func<GameMode> _getStartGameModeObject;
        private readonly Func<GameMode> _getRollOntoScreenObject;
        private readonly string _creditMessageText;

		private int _countDown;
        private bool _releaseWaiting = true;
        private bool _firstCycle = true;



        public TitleScreenWithCredit(
            int titleScreenRollCycles,
            SpriteTraits titleScreenBackground,
            Sound.SoundTraits introSound,
            Font font, string creditMessageText,
            Func<GameMode> getStartGameModeObject,
            Func<GameMode> getRollOntoScreenObject)
        {
            _creditMessageText = creditMessageText;
            _getStartGameModeObject = getStartGameModeObject;
            _getRollOntoScreenObject = getRollOntoScreenObject;
            _font = font;
            _titleScreenBackground = titleScreenBackground;
            _introSound = introSound;
            _countDown = titleScreenRollCycles;
            _titleScreenRollCycles = titleScreenRollCycles;
            _fireButtonPressEnableTime = (titleScreenRollCycles * 3) / 4;
        }



        public override void AdvanceOneCycle(KeyStates theKeyStates)
        {
            if (_firstCycle)
            {
                _introSound.PlayAsBackgroundMusic();
                _firstCycle = false;
            }

            if (theKeyStates.Fire)
            {
                if (_releaseWaiting || _countDown > _fireButtonPressEnableTime) return;
                ActiveMode = _getStartGameModeObject();
            }

            _releaseWaiting = false;

            if (_countDown > 0)
            {
                --_countDown;
            }
            else if (_getRollOntoScreenObject != null)
            {
                ActiveMode = _getRollOntoScreenObject();  // time to leave this mode  eg: a sequence of modes for rolling titles.
            }
        }



        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawSprite(0, 0, _titleScreenBackground.GetHostImageObject(0));
            if (_countDown < _titleScreenRollCycles / 2)
            {
                drawingTarget.DrawText(310, 230, _creditMessageText, _font, TextAlignment.Right);
            }
        }
    }
}
