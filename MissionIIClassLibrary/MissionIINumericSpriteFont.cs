
using System.Collections.Generic;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary
{
    public class MissionIINumericSpriteFont
    {
        public MissionIINumericSpriteFont()
        {
            theNumbers.Add(MissionIISprites.Font0);
            theNumbers.Add(MissionIISprites.Font1);
            theNumbers.Add(MissionIISprites.Font2);
            theNumbers.Add(MissionIISprites.Font3);
            theNumbers.Add(MissionIISprites.Font4);
            theNumbers.Add(MissionIISprites.Font5);
            theNumbers.Add(MissionIISprites.Font6);
            theNumbers.Add(MissionIISprites.Font7);
            theNumbers.Add(MissionIISprites.Font8);
            theNumbers.Add(MissionIISprites.Font9);
        }

        public List<SpriteTraits> theNumbers;
    }
}
