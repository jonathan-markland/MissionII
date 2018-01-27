
using System.Collections.Generic;

namespace MissionIIClassLibrary
{
    public class MissionIINumericSpriteFont
    {
        public MissionIINumericSpriteFont()
        {
            theNumbers.Add(MissionIISpriteTraits.Font0);
            theNumbers.Add(MissionIISpriteTraits.Font1);
            theNumbers.Add(MissionIISpriteTraits.Font2);
            theNumbers.Add(MissionIISpriteTraits.Font3);
            theNumbers.Add(MissionIISpriteTraits.Font4);
            theNumbers.Add(MissionIISpriteTraits.Font5);
            theNumbers.Add(MissionIISpriteTraits.Font6);
            theNumbers.Add(MissionIISpriteTraits.Font7);
            theNumbers.Add(MissionIISpriteTraits.Font8);
            theNumbers.Add(MissionIISpriteTraits.Font9);
        }

        public List<SpriteTraits> theNumbers;
    }
}
