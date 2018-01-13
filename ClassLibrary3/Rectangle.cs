using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public struct Rectangle
    {
        public int Left { get; private set; }
        public int Top { get; private set; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public Rectangle( int left, int top, int width, int height )
        {
            Left = left;
            Top = top;
            Width = width;
            Height = height;
        }

        public int Right
        {
            get { return Left + Width; }
        }

        public int Bottom
        {
            get { return Top + Height; }
        }

        public bool Intersects(Rectangle otherRectangle)
        {
            if (Right <= otherRectangle.Left) return false;
            if (Bottom <= otherRectangle.Top) return false;
            if (Left >= otherRectangle.Right) return false;
            if (Top >= otherRectangle.Bottom) return false;
            return true;
        }

        public Point Centre
        {
            get { return new Point((Left + Right) / 2, (Top + Bottom) / 2); }
        }

        public Rectangle Inflate(int n)
        {
            return new Rectangle(Left - n, Top - n, Width + (2 * n), Height + (2 * n));
        }
    }
}
