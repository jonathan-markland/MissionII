
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
                        + "SOUNDS FROM FREESOUND WEBSITE\v"

                        + "USE CURSOR KEYS\n"
                        + "Z  FIRE  AND  START\n"
                        + "P  PAUSE  AND  LEVEL SELECT\n\n"
                        + "OR JOYSTICK OR PAD\n\n\n"
                        + "F11  OR  F12   FULL SCREEN TOGGLE\v"
                        // + "F2 F3     VIEW SIZE         \v"

                        + "COLLECT OBJECTS ON LEVEL\n"
                        + "THEN FIND THE EXIT\n"
                        + "AVOID ELECTROCUTION\n\n\n"
                        + "SELECT LEVEL IN PAUSE MODE\n"
                        + "BY ENTERING PASS CODE USING\n"
                        + "DIRECTIONS AND FIRE BUTTON\n",
                          (Constants.TitleScreenRollCycles*3)/2,
                          () => StartNewGame.New(),
                          () => TitleScreen.New());
        }
    }
}
