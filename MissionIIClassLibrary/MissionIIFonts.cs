
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
            NarrowFont = new Font(MissionIISprites.FontSprite, charWidth: 6, scaleFactorX: 1, scaleFactorY: 1);
            WideFont   = new Font(MissionIISprites.FontSprite, charWidth: 6, scaleFactorX: 2, scaleFactorY: 1);
            GiantFont  = new Font(MissionIISprites.FontSprite, charWidth: 6, scaleFactorX: 3, scaleFactorY: 4);
        }
    }
}
