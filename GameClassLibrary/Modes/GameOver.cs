
using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace GameClassLibrary.Modes
{
    public class GameOver : GameMode
    {
        private int _countDown;
        private bool _firstCycle = true;
        private Sound.SoundTraits _gameOverSound;
        private SpriteTraits _gameOverSprite;
        private Func<GameMode> _getRollOntoScreenObject;



        public GameOver(
            int gameOverRollCycles,
            SpriteTraits gameOverSprite,
            Sound.SoundTraits gameOverSound,
            Func<GameMode> getRollOntoScreenObject)
        {
            _getRollOntoScreenObject = getRollOntoScreenObject;
            _gameOverSprite = gameOverSprite;
            _gameOverSound = gameOverSound;
            _countDown = gameOverRollCycles;
        }



        public override void AdvanceOneCycle(KeyStates theKeyStates)
        {
            if (_firstCycle)
            {
                _firstCycle = false;
                _gameOverSound.Play();
            }

            if (_countDown > 0)
            {
                --_countDown;
            }
            else
            {
                ActiveMode = _getRollOntoScreenObject();
            }
        }



        public override void Draw(IDrawingTarget drawingTarget)
        {
            drawingTarget.ClearScreen();
            drawingTarget.DrawFirstSpriteScreenCentred(_gameOverSprite);
        }
    }
}
