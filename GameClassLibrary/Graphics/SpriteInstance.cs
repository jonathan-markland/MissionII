
using GameClassLibrary.Math;

namespace GameClassLibrary.Graphics
{
    public class SpriteInstance
    {
        /// <summary>
        /// Horizontal position of sprite.
        /// </summary>
        public int X;

        /// <summary>
        /// Vertical position of sprite.
        /// </summary>
        public int Y;

        /// <summary>
        /// Defines the graphic image, or images, for this sprite instance.
        /// </summary>
        public SpriteTraits Traits;

        public Rectangle GetBoundingRectangle()
        {
            return new Rectangle(X, Y, Traits.BoardWidth, Traits.BoardHeight);
        }

        public bool Intersects(SpriteInstance otherSprite)
        {
            return GetBoundingRectangle().Intersects(otherSprite.GetBoundingRectangle());
        }

        public Point Centre
        {
            get { return GetBoundingRectangle().Centre; }
        }

        public Point TopLeftPosition
        {
            get { return new Point(X, Y); }
            set { X = value.X; Y = value.Y; }
        }

        public void MoveBy(MovementDeltas movementDeltas)
        {
            X += movementDeltas.dx;
            Y += movementDeltas.dy;
        }
    }
}
