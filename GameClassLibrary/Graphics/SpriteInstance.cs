
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

        /// <summary>
        /// Returns the bounding extents rectangle of this sprite, at its current position.
        /// </summary>
        public Rectangle Extents
        {
            get { return new Rectangle(X, Y, Traits.Width, Traits.Height); }
        }

        /// <summary>
        /// Returns true if this sprite intersects the Rectangle.  False otherwise.
        /// </summary>
        public bool Intersects(Rectangle theArea)
        {
            return theArea.Intersects(Extents);
        }

        /// <summary>
        /// Returns true if this sprite intersects the other sprite.  False otherwise.
        /// </summary>
        public bool Intersects(SpriteInstance otherSprite)
        {
            return Intersects(otherSprite.Extents);
        }

        /// <summary>
        /// Returns the centre point of this sprite at its current position.
        /// </summary>
        public Point Centre
        {
            get { return Extents.Centre; }
        }

        /// <summary>
        /// This sprite's position, defined by the position of the top left 
        /// corner of its extents rectangle.
        /// </summary>
        public Point TopLeftPosition
        {
            get { return new Point(X, Y); }
            set { X = value.X; Y = value.Y; }
        }

        /// <summary>
        /// Move the sprite by the given movement delta.
        /// </summary>
        public void MoveBy(MovementDeltas movementDeltas)
        {
            X += movementDeltas.dx;
            Y += movementDeltas.dy;
        }
    }
}
