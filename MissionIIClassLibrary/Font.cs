using System;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary
{
    public class Font
    {
        public SpriteTraits FontSprite;
        public int CharWidth;
        public int ScaleFactor;

        public int CharToIndex(char ch)
        {
            if (ch >= '0' && ch <= '9')
            {
                return ch - '0';
            }
            else if (ch >= 'A' && ch <= 'Z')
            {
                return (ch - 'A') + 10;
            }
            else if (ch >= 'a' && ch <= 'z')
            {
                return (ch - 'a') + 10;
            }
            else return -1;
        }
    }
}
