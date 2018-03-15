
using System.Collections.Generic;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary
{
    public static class MissionIISprites
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
        public static SpriteTraits Ghost;
        public static SpriteTraits GhostStunned;
        public static SpriteTraits Gold;
        public static SpriteTraits Key;
        public static SpriteTraits Life;
        public static SpriteTraits Monster1;
        public static SpriteTraits Monster2;
        public static SpriteTraits Monster3;
        public static SpriteTraits Monster4;
        public static SpriteTraits Monster5;
        public static SpriteTraits Potion;
        public static SpriteTraits Ring;
        public static SpriteTraits LevelExit;
        public static SpriteTraits FloorTile;
        public static SpriteTraits WallOutline;
        public static SpriteTraits WallBrick;
        public static SpriteTraits WallElectric;
        public static SpriteTraits TitleScreen;
        public static SpriteTraits Background;
        public static SpriteTraits HiScoreScreen;
        public static SpriteTraits GameOver;
        public static SpriteTraits Paused;
        public static SpriteTraits PatternResamplingSprite;
        public static SpriteTraits FontSprite;
        public static SpriteTraits InvincibilityAmulet;

        // And references to the above, for convenience:
        public static List<SpriteTraits> ManStanding;
        public static List<SpriteTraits> ManWalking;

        /// <summary>
        /// Loads all the images for MissionII.
        /// </summary>
        public static void Load()
        {
            Bullet = new SpriteTraits("Bullet", 1);
            Dead = new SpriteTraits("Dead", 1);
            Electrocution = new SpriteTraits("Electrocution", 2);
            Explosion = new SpriteTraits("Explosion", 3);
            FacingDown = new SpriteTraits("FacingDown", 1);
            FacingLeft = new SpriteTraits("FacingLeft", 1);
            FacingLeftDown = new SpriteTraits("FacingLeftDown", 1);
            FacingLeftUp = new SpriteTraits("FacingLeftUp", 1);
            FacingRight = new SpriteTraits("FacingRight", 1);
            FacingRightDown = new SpriteTraits("FacingRightDown", 1);
            FacingRightUp = new SpriteTraits("FacingRightUp", 1);
            FacingUp = new SpriteTraits("FacingUp", 1);
            WalkingDown = new SpriteTraits("WalkingDown", 2);
            WalkingLeft = new SpriteTraits("WalkingLeft", 2);
            WalkingLeftDown = new SpriteTraits("WalkingLeftDown", 2);
            WalkingLeftUp = new SpriteTraits("WalkingLeftUp", 2);
            WalkingRight = new SpriteTraits("WalkingRight", 2);
            WalkingRightDown = new SpriteTraits("WalkingRightDown", 2);
            WalkingRightUp = new SpriteTraits("WalkingRightUp", 2);
            WalkingUp = new SpriteTraits("WalkingUp", 2);
            Ghost = new SpriteTraits("Ghost", 1);
            GhostStunned = new SpriteTraits("GhostStunned", 1);
            Gold = new SpriteTraits("Gold", 1);
            Key = new SpriteTraits("Key", 1);
            Life = new SpriteTraits("Life", 1);
            Monster1 = new SpriteTraits("Monster1", 2);
            Monster2 = new SpriteTraits("Monster2", 2);
            Monster3 = new SpriteTraits("Monster3", 1);
            Monster4 = new SpriteTraits("Monster4", 2);
            Monster5 = new SpriteTraits("Monster5", 2);
            Potion = new SpriteTraits("Potion", 2);
            Ring = new SpriteTraits("Ring", 1);
            LevelExit = new SpriteTraits("LevelExit", 1);
            FloorTile = new SpriteTraits("FloorTile", 3);
            WallOutline = new SpriteTraits("WallOutline", 3);
            WallBrick = new SpriteTraits("WallBrick", 3);
            WallElectric = new SpriteTraits("WallElectric", 1);
            HiScoreScreen = new SpriteTraits("HiScoreScreen", 1);
            TitleScreen = new SpriteTraits("TitleScreen", 1);
            Background = new SpriteTraits("Background", 1);
            GameOver = new SpriteTraits("GameOver", 1);
            Paused = new SpriteTraits("Paused", 1);
            PatternResamplingSprite = new SpriteTraits("PatternResamplingSprite", 4);
            FontSprite = new SpriteTraits("Font", 1);
            InvincibilityAmulet = new SpriteTraits("InvincibilityAmulet", 1);
            FacingDown = new SpriteTraits("FacingDown", 1);
            FacingLeft = new SpriteTraits("FacingLeft", 1);
            FacingLeftDown = new SpriteTraits("FacingLeftDown", 1);
            FacingLeftUp = new SpriteTraits("FacingLeftUp", 1);
            FacingRight = new SpriteTraits("FacingRight", 1);
            FacingRightDown = new SpriteTraits("FacingRightDown", 1);
            FacingRightUp = new SpriteTraits("FacingRightUp", 1);
            FacingUp = new SpriteTraits("FacingUp", 1);
            WalkingDown = new SpriteTraits("WalkingDown", 2);
            WalkingLeft = new SpriteTraits("WalkingLeft", 2);
            WalkingLeftDown = new SpriteTraits("WalkingLeftDown", 2);
            WalkingLeftUp = new SpriteTraits("WalkingLeftUp", 2);
            WalkingRight = new SpriteTraits("WalkingRight", 2);
            WalkingRightDown = new SpriteTraits("WalkingRightDown", 2);
            WalkingRightUp = new SpriteTraits("WalkingRightUp", 2);
            WalkingUp = new SpriteTraits("WalkingUp", 2);

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
        }
    }
}
