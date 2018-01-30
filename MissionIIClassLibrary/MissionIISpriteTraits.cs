using System;
using System.Collections.Generic;

namespace MissionIIClassLibrary
{
    public static class MissionIISpriteTraits
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
        public static SpriteTraits GhostStunned;
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
        public static SpriteTraits FloorTile;
        public static SpriteTraits WallOutline;
        public static SpriteTraits WallBrick;
        public static SpriteTraits WallElectric;
        public static SpriteTraits TitleScreen;
        public static SpriteTraits EnteringLevel;
        public static SpriteTraits GameOver;
        public static SpriteTraits Paused;
        public static SpriteTraits PatternResamplingSprite;

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
            Func<string, int, SpriteTraits> loadImage = (spriteName, imageCount) =>
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

                return new SpriteTraits(boardWidth, boardHeight, hostImageObjects);
            };

            Bullet = loadImage("Bullet", 1);
            Dead = loadImage("Dead", 1);
            Electrocution = loadImage("Electrocution", 2);
            Explosion = loadImage("Explosion", 3);
            FacingDown = loadImage("FacingDown", 1);
            FacingLeft = loadImage("FacingLeft", 1);
            FacingLeftDown = loadImage("FacingLeftDown", 1);
            FacingLeftUp = loadImage("FacingLeftUp", 1);
            FacingRight = loadImage("FacingRight", 1);
            FacingRightDown = loadImage("FacingRightDown", 1);
            FacingRightUp = loadImage("FacingRightUp", 1);
            FacingUp = loadImage("FacingUp", 1);
            WalkingDown = loadImage("WalkingDown", 2);
            WalkingLeft = loadImage("WalkingLeft", 2);
            WalkingLeftDown = loadImage("WalkingLeftDown", 2);
            WalkingLeftUp = loadImage("WalkingLeftUp", 2);
            WalkingRight = loadImage("WalkingRight", 2);
            WalkingRightDown = loadImage("WalkingRightDown", 2);
            WalkingRightUp = loadImage("WalkingRightUp", 2);
            WalkingUp = loadImage("WalkingUp", 2);
            Font0 = loadImage("Font0", 1);
            Font1 = loadImage("Font1", 1);
            Font2 = loadImage("Font2", 1);
            Font3 = loadImage("Font3", 1);
            Font4 = loadImage("Font4", 1);
            Font5 = loadImage("Font5", 1);
            Font6 = loadImage("Font6", 1);
            Font7 = loadImage("Font7", 1);
            Font8 = loadImage("Font8", 1);
            Font9 = loadImage("Font9", 1);
            Ghost = loadImage("Ghost", 1);
            GhostStunned = loadImage("GhostStunned", 1);
            Gold = loadImage("Gold", 1);
            Key = loadImage("Key", 1);
            Life = loadImage("Life", 1);
            Monster1 = loadImage("Monster1", 2);
            Monster2 = loadImage("Monster2", 2);
            Monster3 = loadImage("Monster3", 1);
            Potion = loadImage("Potion", 2);
            Ring = loadImage("Ring", 1);
            Room = loadImage("Room", 1);
            Safe = loadImage("Safe", 1);
            Score = loadImage("Score", 1);
            FloorTile = loadImage("FloorTile", 3);
            WallOutline = loadImage("WallOutline", 3);
            WallBrick = loadImage("WallBrick", 3);
            WallElectric = loadImage("WallElectric", 1);
            TitleScreen = loadImage("TitleScreen", 5);
            EnteringLevel = loadImage("EnteringLevel", 1);
            GameOver = loadImage("GameOver", 1);
            Paused = loadImage("Paused", 1);
            PatternResamplingSprite = loadImage("PatternResamplingSprite", 1);


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

            FacingDown = loadImage("FacingDown", 1);
            FacingLeft = loadImage("FacingLeft", 1);
            FacingLeftDown = loadImage("FacingLeftDown", 1);
            FacingLeftUp = loadImage("FacingLeftUp", 1);
            FacingRight = loadImage("FacingRight", 1);
            FacingRightDown = loadImage("FacingRightDown", 1);
            FacingRightUp = loadImage("FacingRightUp", 1);
            FacingUp = loadImage("FacingUp", 1);
            WalkingDown = loadImage("WalkingDown", 2);
            WalkingLeft = loadImage("WalkingLeft", 2);
            WalkingLeftDown = loadImage("WalkingLeftDown", 2);
            WalkingLeftUp = loadImage("WalkingLeftUp", 2);
            WalkingRight = loadImage("WalkingRight", 2);
            WalkingRightDown = loadImage("WalkingRightDown", 2);
            WalkingRightUp = loadImage("WalkingRightUp", 2);
            WalkingUp = loadImage("WalkingUp", 2);

        }
    }
}
