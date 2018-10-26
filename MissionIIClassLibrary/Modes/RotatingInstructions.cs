
using GameClassLibrary.Modes;

namespace MissionIIClassLibrary.Modes
{
    public static class MissionRotatingInstructions
    {
        public static ModeFunctions New()
        {
            return GameClassLibrary.Modes.RotatingInstructions.New(
                MissionIISprites.Background,
                        MissionIIFonts.NarrowFont,
                          "WRITTEN BY JONATHAN MARKLAND\n"
                        + "BASED ON AN\n"
                        + "ORIGINAL CONCEPT\n"
                        + "BY MATTHEW BATES\n\n"
                        + "SOUNDS FROM FREESOUND WEBSITE\n\n\n"
                        + "USE CURSOR KEYS AND Z TO FIRE\v"

                        + "USE CURSOR KEYS\n"
                        + "       Z  FIRE \n"
                        + "       P  PAUSE\n\n"
                        + "OR JOYSTICK OR PAD\n\n\n"
                        + "F11  OR  F12   FULL SCREEN TOGGLE\v"
                        // + "F2 F3     VIEW SIZE         \v"

                        + "COLLECT OBJECTS ON LEVEL\n"
                        + "THEN FIND THE EXIT\n"
                        + "AVOID ELECTROCUTION\n\n\n"
                        + "USE CURSOR KEYS AND Z TO FIRE",
                          Constants.TitleScreenRollCycles,
                          () => StartNewGame.New(),
                          () => TitleScreen.New());
        }
    }
}
