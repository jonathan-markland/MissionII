using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameClassLibrary.Math;

namespace GameClassLibrary
{
    public class SpriteInstance
    {
        /// <summary>
        /// Position of sprite with respect to left edge of room.
        /// (Not left edge of screen!)
        /// </summary>
        public int RoomX;

        /// <summary>
        /// Position of sprite with respect to top of room.
        /// (Not top edge of screen!)
        /// </summary>
        public int RoomY;

        /// <summary>
        /// Defines the graphic image, or images, for this sprite instance.
        /// </summary>
        public SpriteTraits Traits;

        public Rectangle GetBoundingRectangle()
        {
            return new Rectangle(RoomX, RoomY, Traits.BoardWidth, Traits.BoardHeight);
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
            get { return new Point(RoomX, RoomY); }
            set { RoomX = value.X; RoomY = value.Y; }
        }

        public void MoveBy(MovementDeltas movementDeltas)
        {
            RoomX += movementDeltas.dx;
            RoomY += movementDeltas.dy;
        }
    }
}
