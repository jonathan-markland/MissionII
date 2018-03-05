
using System;
using GameClassLibrary.Graphics;
using System.Collections.Generic;

namespace MissionIIClassLibrary
{
    public class WallAndFloorHostSprites
    {
        public HostSuppliedSprite[] OutlineBricks { get; private set; }  // Has 2 items
        public HostSuppliedSprite[] FillerBricks { get; private set; }   // Has 2 items
        public HostSuppliedSprite[] FloorBricks { get; private set; }    // Has 2 items



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
                new HostSuppliedSprite[]
                {
                    outlineSpriteTraits.GetHostImageObject(levelNumber % outlineSpriteTraits.ImageCount),
                    outlineSpriteTraits.GetHostImageObject((levelNumber + 1) % outlineSpriteTraits.ImageCount)
                },
                levelNumber * OutlineBrickLevelSeparation);

            FillerBricks = RecolourByThresholdAndColourWheel(
                new HostSuppliedSprite[]
                {
                    brickSpriteTraits.GetHostImageObject(levelNumber % brickSpriteTraits.ImageCount),
                    brickSpriteTraits.GetHostImageObject((levelNumber + 1) % brickSpriteTraits.ImageCount)
                },
                levelNumber * FillerBrickLevelSeparation);

            var baseBrickGreyLevel = (levelNumber & 1) * FloorBrickLevelSeparation + FloorBrickLevelBase;

            FloorBricks = new HostSuppliedSprite[]
            {
                // First brick type sprite
                RecolourByThresholdAndGreyLevels(
                    floorSpriteTraits.GetHostImageObject(levelNumber % floorSpriteTraits.ImageCount),
                    baseBrickGreyLevel, 
                    baseBrickGreyLevel + GreyLevelSeparation),

                // Second brick type sprite
                RecolourByThresholdAndGreyLevels(
                    floorSpriteTraits.GetHostImageObject((levelNumber + 1) % floorSpriteTraits.ImageCount),
                    baseBrickGreyLevel + GreyLevelSeparation * 3, 
                    baseBrickGreyLevel + GreyLevelSeparation * 4)
            };
        }



        private HostSuppliedSprite[] RecolourByThresholdAndColourWheel(HostSuppliedSprite[] hostSpritesArray, int seedValue)
        {
            var theList = new List<HostSuppliedSprite>();
            foreach(var hostSprite in hostSpritesArray)
            {
                var firstColour = Colour.GetWheelColourAsPackedValue(seedValue);
                var secondColour = Colour.GetWheelColourAsPackedValue(seedValue + TwoColourBrickColourSeparation);
                var imageDataArray = hostSprite.ToArray();
                Colour.ReplaceWithThreshold(imageDataArray, firstColour, secondColour);
                var newHostImage = GameClassLibrary.Graphics.HostSuppliedSprite.UintArrayToSprite(imageDataArray, hostSprite.BoardWidth, hostSprite.BoardHeight);
                theList.Add(newHostImage);
                seedValue += ColourSeparationBetweenColouredBricks;
            }
            return theList.ToArray();
        }



        private HostSuppliedSprite RecolourByThresholdAndGreyLevels(HostSuppliedSprite hostSprite, int lowGreyLevel, int highGreyLevel)
        {
            return RecolourByThresholdAndSpecificColours(
                hostSprite,
                Colour.ToGreyscale((byte) highGreyLevel),
                Colour.ToGreyscale((byte) lowGreyLevel));
        }



        private HostSuppliedSprite RecolourByThresholdAndSpecificColours(HostSuppliedSprite hostSprite, uint highColour, uint lowColour)
        {
            var imageDataArray = hostSprite.ToArray();
            Colour.ReplaceWithThreshold(imageDataArray, highColour, lowColour);
            return HostSuppliedSprite.UintArrayToSprite(imageDataArray, hostSprite.BoardWidth, hostSprite.BoardHeight);
        }
    }
}
