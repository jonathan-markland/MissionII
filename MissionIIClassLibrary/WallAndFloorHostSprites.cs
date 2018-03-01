
using System;
using GameClassLibrary.Graphics;
using System.Collections.Generic;

namespace MissionIIClassLibrary
{
    public class WallAndFloorHostSprites
    {
        public object[] OutlineBricks { get; private set; }  // Has 2 items
        public object[] FillerBricks { get; private set; }   // Has 2 items
        public object[] FloorBricks { get; private set; }    // Has 2 items

        public WallAndFloorHostSprites(
            int levelNumber,
            SpriteTraits outlineSpriteTraits,
            SpriteTraits brickSpriteTraits,
            SpriteTraits floorSpriteTraits)
        {
            --levelNumber; // because it's 1-based!

            var theWidth = outlineSpriteTraits.BoardWidth;
            var theHeight = outlineSpriteTraits.BoardHeight;

            if (brickSpriteTraits.BoardWidth != theWidth
                || brickSpriteTraits.BoardHeight != theHeight
                || floorSpriteTraits.BoardWidth != theWidth
                || floorSpriteTraits.BoardHeight != theHeight)
            {
                throw new Exception("Brick sprite sets aren't the same dimensions!");
            }

            OutlineBricks = new object[]
            {
                outlineSpriteTraits.GetHostImageObject(levelNumber % outlineSpriteTraits.ImageCount),
                outlineSpriteTraits.GetHostImageObject((levelNumber + 1) % outlineSpriteTraits.ImageCount)
            };

            FillerBricks = new object[]
            {
                brickSpriteTraits.GetHostImageObject(levelNumber % brickSpriteTraits.ImageCount),
                brickSpriteTraits.GetHostImageObject((levelNumber + 1) % brickSpriteTraits.ImageCount)
            };

            FloorBricks = new object[]
            {
                floorSpriteTraits.GetHostImageObject(levelNumber % floorSpriteTraits.ImageCount),
                floorSpriteTraits.GetHostImageObject((levelNumber + 1) % floorSpriteTraits.ImageCount)
            };

            var n = levelNumber * 32;
            OutlineBricks = GenerateRecolouredByThreshold(theWidth, theHeight, OutlineBricks, n, 0);
            FillerBricks = GenerateRecolouredByThreshold(theWidth, theHeight, FillerBricks, n + 128, 0);
            FloorBricks = GenerateRecolouredByThreshold(theWidth, theHeight, FillerBricks, n + 128, 3);
        }



        private object[] GenerateRecolouredByThreshold(int theWidth, int theHeight, object[] hostSpritesArray, int seedValue, int dimmingShift)
        {
            var theList = new List<object>();
            foreach(var hostSprite in hostSpritesArray)
            {
                var firstColour = Colour.GetWheelColourAsPackedValue(seedValue);
                var secondColour = Colour.GetWheelColourAsPackedValue(seedValue + 30);

                while (dimmingShift > 0)
                {
                    firstColour &= 0xFEFEFE;
                    secondColour &= 0xFEFEFE;
                    firstColour >>= 1;
                    secondColour >>= 1;
                    firstColour |= 0xFF000000;
                    secondColour |= 0xFF000000;
                    --dimmingShift;
                }

                var imageDataArray = Business.SpriteToUintArray(hostSprite);
                Colour.ReplaceWithThreshold(imageDataArray, firstColour, secondColour);
                var newHostImage = Business.UintArrayToSprite(imageDataArray, theWidth, theHeight);
                theList.Add(newHostImage);

                seedValue += 50;
            }
            return theList.ToArray();
        }
    }
}
