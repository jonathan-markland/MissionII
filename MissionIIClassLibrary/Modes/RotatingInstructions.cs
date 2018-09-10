
namespace MissionIIClassLibrary.Modes
{
    public class MissionRotatingInstructions : GameClassLibrary.Modes.RotatingInstructions
    {
        public MissionRotatingInstructions()
            : base(
                        MissionIISprites.Background,
                        MissionIIFonts.NarrowFont,
                          "VERSION BY JONATHAN MARKLAND\n"
                        + "BASED ON AN\n"
                        + "ORIGINAL CONCEPT\n"
                        + "BY MATTHEW BATES\n\n"
                        + "SOUNDS FROM FREESOUND WEBSITE\v"

                        + "MOVE USING CURSOR KEYS\n"
                        + "Z      FIRE\n"
                        + "P      PAUSE\n"
                        + "OR JOYSTICK OR PAD\n\n"
                        + "F11 F12   FULL SCREEN TOGGLE\n"
                        + "F2 F3     VIEW SIZE\v"

                        + "COLLECT OBJECTS ON LEVEL\n"
                        + "THEN FIND THE EXIT\n"
                        + "AVOID ELECTROCUTION",
                          Constants.TitleScreenRollCycles,
                          () => new StartNewGame(),
                          () => new TitleScreen() 
                    )
        {
        }
    }
}
