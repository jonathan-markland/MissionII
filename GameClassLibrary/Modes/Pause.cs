
using System;
using GameClassLibrary.Graphics;

namespace GameClassLibrary.Modes
{
    public static class Pause  // TODO: Perhaps the wrong way around!
    {
        public static ModeFunctions New(
            ModeFunctions originalMode,
            SpriteTraits pauseSprite,
            Sound.SoundTraits pauseSound,
            Func<ModeFunctions> getNextModeFunction)
        {
            return PauseWithChangeLevel.New(originalMode,
                  pauseSprite,
                  pauseSound,
                  0,
                  null,
                  null,
                  s => false,
                  getNextModeFunction,
                  () => false);
        }
    }
}
