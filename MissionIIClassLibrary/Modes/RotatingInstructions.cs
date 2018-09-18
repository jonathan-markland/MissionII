
namespace MissionIIClassLibrary.Modes
{
    public class MissionRotatingInstructions : GameClassLibrary.Modes.RotatingInstructions
    {
        public MissionRotatingInstructions()
            : base(
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
                          () => new StartNewGame(),
                          () => new TitleScreen() 
                    )
        {
        }
    }
}
