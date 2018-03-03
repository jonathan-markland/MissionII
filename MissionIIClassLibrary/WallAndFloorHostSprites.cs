
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



        private const int GreyLevelSeparation = 10;
        private const int OutlineBrickLevelSeparation = 55;
        private const int FillerBrickLevelSeparation = 35;
        private const int FloorBrickLevelSeparation = 18;
        private const int FloorBrickLevelBase = 25;
        private const int TwoColourBrickColourSeparation = 30;
        private const int ColourSeparationBetweenColouredBricks = 50;



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

            OutlineBricks = RecolourByThresholdAndColourWheel(
                theWidth, theHeight,
                new object[]
                {
                    outlineSpriteTraits.GetHostImageObject(levelNumber % outlineSpriteTraits.ImageCount),
                    outlineSpriteTraits.GetHostImageObject((levelNumber + 1) % outlineSpriteTraits.ImageCount)
                },
                levelNumber * OutlineBrickLevelSeparation);

            FillerBricks = RecolourByThresholdAndColourWheel(
                theWidth, theHeight,
                new object[]
                {
                    brickSpriteTraits.GetHostImageObject(levelNumber % brickSpriteTraits.ImageCount),
                    brickSpriteTraits.GetHostImageObject((levelNumber + 1) % brickSpriteTraits.ImageCount)
                },
                levelNumber * FillerBrickLevelSeparation);

            var baseBrickGreyLevel = (levelNumber & 1) * FloorBrickLevelSeparation + FloorBrickLevelBase;

            FloorBricks = new object[]
            {
                // First brick type sprite
                RecolourByThresholdAndGreyLevels(theWidth, theHeight,
                    floorSpriteTraits.GetHostImageObject(levelNumber % floorSpriteTraits.ImageCount),
                    baseBrickGreyLevel, 
                    baseBrickGreyLevel + GreyLevelSeparation),

                // Second brick type sprite
                RecolourByThresholdAndGreyLevels(theWidth, theHeight,
                    floorSpriteTraits.GetHostImageObject((levelNumber + 1) % floorSpriteTraits.ImageCount),
                    baseBrickGreyLevel + GreyLevelSeparation * 3, 
                    baseBrickGreyLevel + GreyLevelSeparation * 4)
            };
        }



        private object[] RecolourByThresholdAndColourWheel(int theWidth, int theHeight, object[] hostSpritesArray, int seedValue)
        {
            var theList = new List<object>();
            foreach(var hostSprite in hostSpritesArray)
            {
                var firstColour = Colour.GetWheelColourAsPackedValue(seedValue);
                var secondColour = Colour.GetWheelColourAsPackedValue(seedValue + TwoColourBrickColourSeparation);
                var imageDataArray = Business.SpriteToUintArray(hostSprite);
                Colour.ReplaceWithThreshold(imageDataArray, firstColour, secondColour);
                var newHostImage = Business.UintArrayToSprite(imageDataArray, theWidth, theHeight);
                theList.Add(newHostImage);
                seedValue += ColourSeparationBetweenColouredBricks;
            }
            return theList.ToArray();
        }



        private object RecolourByThresholdAndGreyLevels(int theWidth, int theHeight, object hostSprite, int lowGreyLevel, int highGreyLevel)
        {
            return RecolourByThresholdAndSpecificColours(
                theWidth, theHeight, 
                hostSprite,
                Colour.ToGreyscale((byte) highGreyLevel),
                Colour.ToGreyscale((byte) lowGreyLevel));
        }



        private object RecolourByThresholdAndSpecificColours(int theWidth, int theHeight, object hostSprite, uint highColour, uint lowColour)
        {
            var imageDataArray = Business.SpriteToUintArray(hostSprite);
            Colour.ReplaceWithThreshold(imageDataArray, highColour, lowColour);
            return Business.UintArrayToSprite(imageDataArray, theWidth, theHeight);
        }
    }
}
