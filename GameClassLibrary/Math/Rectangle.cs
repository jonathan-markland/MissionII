
using System.Collections.Generic;

namespace GameClassLibrary.Math
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



        public Rectangle(Point topLeft, int width, int height)
        {
            Left = topLeft.X;
            Top = topLeft.Y;
            Width = width;
            Height = height;
        }



        public Rectangle MovedBy(MovementDeltas movementDeltas)
        {
            return new Rectangle(Left + movementDeltas.dx, Top + movementDeltas.dy, Width, Height);
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



        public bool Intersects(List<Rectangle> theList)
        {
            foreach(var r in theList)
            {
                if (Intersects(r)) return true;
            }
            return false;
        }
    }
}
