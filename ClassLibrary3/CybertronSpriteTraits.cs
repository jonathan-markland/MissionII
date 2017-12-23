using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
{
    public class HostSuppliedSprite
    {
        public int BoardWidth;
        public int BoardHeight;
        public object HostObject;
    }

    public static class CybertronSpriteTraits
    {
        // TODO: really should be internal
        public static SpriteTraits Bullet;
        public static SpriteTraits Dead;
        public static SpriteTraits Electrocution;
        public static SpriteTraits Explosion;
        public static SpriteTraits FacingDown;
        public static SpriteTraits FacingLeft;
        public static SpriteTraits FacingLeftDown;
        public static SpriteTraits FacingLeftUp;
        public static SpriteTraits FacingRight;
        public static SpriteTraits FacingRightDown;
        public static SpriteTraits FacingRightUp;
        public static SpriteTraits FacingUp;
        public static SpriteTraits WalkingDown;
        public static SpriteTraits WalkingLeft;
        public static SpriteTraits WalkingLeftDown;
        public static SpriteTraits WalkingLeftUp;
        public static SpriteTraits WalkingRight;
        public static SpriteTraits WalkingRightDown;
        public static SpriteTraits WalkingRightUp;
        public static SpriteTraits WalkingUp;
        public static SpriteTraits Font0;
        public static SpriteTraits Font1;
        public static SpriteTraits Font2;
        public static SpriteTraits Font3;
        public static SpriteTraits Font4;
        public static SpriteTraits Font5;
        public static SpriteTraits Font6;
        public static SpriteTraits Font7;
        public static SpriteTraits Font8;
        public static SpriteTraits Font9;
        public static SpriteTraits Ghost;
        public static SpriteTraits Gold;
        public static SpriteTraits Key;
        public static SpriteTraits Life;
        public static SpriteTraits Monster1;
        public static SpriteTraits Monster2;
        public static SpriteTraits Monster3;
        public static SpriteTraits Potion;
        public static SpriteTraits Ring;
        public static SpriteTraits Room;
        public static SpriteTraits Safe;
        public static SpriteTraits Score;
        public static SpriteTraits WallBlock;

        // And references to the above, for convenience:
        public static List<SpriteTraits> TheNumbers;
        public static List<SpriteTraits> ManStanding;
        public static List<SpriteTraits> ManWalking;

        /// <summary>
        /// Loads all the images for Cybertron by calling back into the host
        /// and passing each individual image name in turn, and expecting the
        /// host to return a HostSuppliedSprite object, describing each.
        /// </summary>
        public static void Load(Func<string, HostSuppliedSprite> hostSpriteSupplier)
        {
            Func<string, int, GameTimeSpan, SpriteTraits> loadImage = (spriteName, imageCount, timeBetweenFrames) =>
            {
                // When there are multiple images:
                // - We append "_1", "_2" .. etc to the spriteName
                //   and request those names from the host.
                // - We must also make sure all images in the set are the same size!
                // When it's just a single image, we just load that only by the name given.

                int boardWidth = 0;
                int boardHeight = 0;
                var hostImageObjects = new List<object>();

                for( int i = 1; i <= imageCount; i++ )
                {
                    var thisHostImageInfo = hostSpriteSupplier((imageCount == 1) ? spriteName : spriteName + "_" + i);
                    if (i == 1)
                    {
                        boardWidth = thisHostImageInfo.BoardWidth;
                        boardHeight = thisHostImageInfo.BoardHeight;
                    }
                    else
                    {
                        if (boardWidth != thisHostImageInfo.BoardWidth)
                        {
                            throw new Exception("Sprite widths don't match in the file set for '" + spriteName + "'.");
                        }
                        if (boardHeight != thisHostImageInfo.BoardHeight)
                        {
                            throw new Exception("Sprite heights don't match in the file set for '" + spriteName + "'.");
                        }
                    }
                    hostImageObjects.Add(thisHostImageInfo.HostObject);
                }

                return new SpriteTraits(boardWidth, boardHeight, hostImageObjects, timeBetweenFrames);
            };

            var notApplicable = new GameTimeSpan { Milliseconds = 0 };
            var twentyMs = new GameTimeSpan { Milliseconds = 20 };
            var oneHundredMs = new GameTimeSpan { Milliseconds = 100 };
            var walkSpeedMs = new GameTimeSpan { Milliseconds = 250 };

            Bullet = loadImage("Bullet", 1, notApplicable);
            Dead = loadImage("Dead", 1, notApplicable);
            Electrocution = loadImage("Electrocution", 2, twentyMs);
            Explosion = loadImage("Explosion", 3, twentyMs);
            FacingDown = loadImage("FacingDown", 1, notApplicable);
            FacingLeft = loadImage("FacingLeft", 1, notApplicable);
            FacingLeftDown = loadImage("FacingLeftDown", 1, notApplicable);
            FacingLeftUp = loadImage("FacingLeftUp", 1, notApplicable);
            FacingRight = loadImage("FacingRight", 1, notApplicable);
            FacingRightDown = loadImage("FacingRightDown", 1, notApplicable);
            FacingRightUp = loadImage("FacingRightUp", 1, notApplicable);
            FacingUp = loadImage("FacingUp", 1, notApplicable);
            WalkingDown = loadImage("WalkingDown", 2, walkSpeedMs);
            WalkingLeft = loadImage("WalkingLeft", 2, walkSpeedMs);
            WalkingLeftDown = loadImage("WalkingLeftDown", 2, walkSpeedMs);
            WalkingLeftUp = loadImage("WalkingLeftUp", 2, walkSpeedMs);
            WalkingRight = loadImage("WalkingRight", 2, walkSpeedMs);
            WalkingRightDown = loadImage("WalkingRightDown", 2, walkSpeedMs);
            WalkingRightUp = loadImage("WalkingRightUp", 2, walkSpeedMs);
            WalkingUp = loadImage("WalkingUp", 2, walkSpeedMs);
            Font0 = loadImage("Font0", 1, notApplicable);
            Font1 = loadImage("Font1", 1, notApplicable);
            Font2 = loadImage("Font2", 1, notApplicable);
            Font3 = loadImage("Font3", 1, notApplicable);
            Font4 = loadImage("Font4", 1, notApplicable);
            Font5 = loadImage("Font5", 1, notApplicable);
            Font6 = loadImage("Font6", 1, notApplicable);
            Font7 = loadImage("Font7", 1, notApplicable);
            Font8 = loadImage("Font8", 1, notApplicable);
            Font9 = loadImage("Font9", 1, notApplicable);
            Ghost = loadImage("Ghost", 1, notApplicable);
            Gold = loadImage("Gold", 1, notApplicable);
            Key = loadImage("Key", 1, notApplicable);
            Life = loadImage("Life", 1, notApplicable);
            Monster1 = loadImage("Monster1", 2, oneHundredMs);
            Monster2 = loadImage("Monster2", 2, oneHundredMs);
            Monster3 = loadImage("Monster3", 1, notApplicable);
            Potion = loadImage("Potion", 2, oneHundredMs);
            Ring = loadImage("Ring", 1, notApplicable);
            Room = loadImage("Room", 1, notApplicable);
            Safe = loadImage("Safe", 1, notApplicable);
            Score = loadImage("Score", 1, notApplicable);
            WallBlock = loadImage("WallBlock", 1, notApplicable);

            // Now build a list of the numbers for convenience of the drawing routines:
            TheNumbers = new List<SpriteTraits>
            {
                Font0,
                Font1,
                Font2,
                Font3,
                Font4,
                Font5,
                Font6,
                Font7,
                Font8,
                Font9
            };

            ManStanding = new List<SpriteTraits>
            {
                FacingUp,
                FacingRightUp,
                FacingRight,
                FacingRightDown,
                FacingDown,
                FacingLeftDown,
                FacingLeft,
                FacingLeftUp
            };

            ManWalking = new List<SpriteTraits>
            {
                WalkingUp,
                WalkingRightUp,
                WalkingRight,
                WalkingRightDown,
                WalkingDown,
                WalkingLeftDown,
                WalkingLeft,
                WalkingLeftUp
            };

            FacingDown = loadImage("FacingDown", 1, notApplicable);
            FacingLeft = loadImage("FacingLeft", 1, notApplicable);
            FacingLeftDown = loadImage("FacingLeftDown", 1, notApplicable);
            FacingLeftUp = loadImage("FacingLeftUp", 1, notApplicable);
            FacingRight = loadImage("FacingRight", 1, notApplicable);
            FacingRightDown = loadImage("FacingRightDown", 1, notApplicable);
            FacingRightUp = loadImage("FacingRightUp", 1, notApplicable);
            FacingUp = loadImage("FacingUp", 1, notApplicable);
            WalkingDown = loadImage("WalkingDown", 2, walkSpeedMs);
            WalkingLeft = loadImage("WalkingLeft", 2, walkSpeedMs);
            WalkingLeftDown = loadImage("WalkingLeftDown", 2, walkSpeedMs);
            WalkingLeftUp = loadImage("WalkingLeftUp", 2, walkSpeedMs);
            WalkingRight = loadImage("WalkingRight", 2, walkSpeedMs);
            WalkingRightDown = loadImage("WalkingRightDown", 2, walkSpeedMs);
            WalkingRightUp = loadImage("WalkingRightUp", 2, walkSpeedMs);
            WalkingUp = loadImage("WalkingUp", 2, walkSpeedMs);

        }
    }
}
