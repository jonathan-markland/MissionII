using System;

namespace GameClassLibrary.Graphics
{
    public class Font
    {
        public SpriteTraits FontSprite;
        public int CharWidth;
        public int ScaleFactor;

        public int Height { get { return FontSprite.BoardHeight; } }

        public static int CharToIndex(char ch)
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

        public static char IndexToChar(int i)
        {
            if (i >= 0 && i <= 9) return (char)(48 + i);
            if (i >= 10 && i <= 35) return (char)(65 + i);
            return ' ';
        }
    }
}
