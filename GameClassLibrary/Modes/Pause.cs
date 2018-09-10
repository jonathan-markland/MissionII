
using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;
using GameClassLibrary.Controls;

namespace GameClassLibrary.Modes
{
    public class Pause : PauseWithChangeLevel  // TODO: Perhaps the wrong way around!
    {
        public Pause(
            GameMode originalMode,
            SpriteTraits pauseSprite,
            Sound.SoundTraits pauseSound,
            Func<GameMode> getNextModeFunction)
            : base(
                  originalMode, 
                  pauseSprite,
                  pauseSound,
                  0,
                  null,
                  null,
                  s => false,
                  getNextModeFunction,
                  () => false)
        {
        }
    }
}
