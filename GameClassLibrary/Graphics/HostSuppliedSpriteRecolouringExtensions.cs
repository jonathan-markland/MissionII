
namespace GameClassLibrary.Graphics
{
    public static class HostSuppliedSpriteRecolouringExtensions
    {
        /// <summary>
        /// Return a re-coloured version of the source sprite where:
        /// Source blue == 0 means use BLACK.
        /// Source blue >= 128 means use the high colour,
        /// else use the low colour.
        /// </summary>
        public static HostSuppliedSprite RecolourByThreshold(
            this HostSuppliedSprite hostSprite,
            uint highColourARGB,
            uint lowColourARGB)
        {
            var imageDataArray = hostSprite.PixelsToUintArray();
            Colour.ReplaceWithThreshold(imageDataArray, highColourARGB, lowColourARGB);
            return new HostSuppliedSprite(imageDataArray, hostSprite.Width, hostSprite.Height);
        }



        /// <summary>
        /// Return a re-coloured version of the source sprite where:
        /// Source blue == 0 means use BLACK.
        /// Source blue >= 128 means use the high colour,
        /// else use the low colour.
        /// The low and high colours are given by indexes (0..255) into the
        /// RGB colour wheel.
        /// </summary>
        public static HostSuppliedSprite RecolourByThresholdAndColourWheel(
            this HostSuppliedSprite hostSprite,
            int highColourWheelIndex,
            int lowColourWheelIndex)
        {
            var highColour = Colour.GetWheelColourAsPackedValue(highColourWheelIndex);
            var lowColour = Colour.GetWheelColourAsPackedValue(lowColourWheelIndex);
            return hostSprite.RecolourByThreshold(highColour, lowColour);
        }



        /// <summary>
        /// Return a re-coloured version of the source sprite where:
        /// Source blue == 0 means use BLACK.
        /// Source blue >= 128 means use the high colour,
        /// else use the low colour.
        /// The low and high colours are given by grey levels (0..255).
        /// The result image is two-tone greyscale, plus black.
        /// </summary>
        public static HostSuppliedSprite RecolourByThresholdAndGreyLevels(
            this HostSuppliedSprite hostSprite, int lowGreyLevel, int highGreyLevel)
        {
            return hostSprite.RecolourByThreshold(
                Colour.ExpandToGreyScaleArgb((byte)highGreyLevel),
                Colour.ExpandToGreyScaleArgb((byte)lowGreyLevel));
        }
    }
}
