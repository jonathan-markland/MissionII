
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary
{
    public static class MissionIIFonts
    {
        public static Font NarrowFont;
        public static Font WideFont;
        public static Font GiantFont;

        /// <summary>
        /// Loads all the fonts for MissionII.
        /// </summary>
        public static void Load()
        {
            NarrowFont = new Font { FontSprite = MissionIISprites.FontSprite, CharWidth = 6, ScaleFactorX = 1, ScaleFactorY = 1 };
            WideFont   = new Font { FontSprite = MissionIISprites.FontSprite, CharWidth = 6, ScaleFactorX = 2, ScaleFactorY = 1 };
            GiantFont  = new Font { FontSprite = MissionIISprites.FontSprite, CharWidth = 6, ScaleFactorX = 3, ScaleFactorY = 4 };
        }
    }
}
