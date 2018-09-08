
using System;
using System.Collections.Generic;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary
{
    public static class ColouredTileSpriteGenerator
    {
        private const int GreyLevelSeparation = 10;
        private const int ElectricBrickLevelSeparation = 55;
        private const int WallBrickLevelSeparation = 35;
        private const int FloorBrickLevelSeparation = 18;
        private const int FloorBrickLevelBase = 25;
        private const int TwoColourBrickColourSeparation = 30;
        private const int ColourSeparationBetweenColouredBricks = 50;



        public static HostSuppliedSprite[] GenerateImages(
            int levelNumber,
            SpriteTraits outlineSpriteTraits,
            SpriteTraits brickSpriteTraits,
            SpriteTraits floorSpriteTraits)
        {
            --levelNumber; // because it's 1-based!

            var theWidth = outlineSpriteTraits.Width;
            var theHeight = outlineSpriteTraits.Height;

            if (brickSpriteTraits.Width != theWidth
                || brickSpriteTraits.Height != theHeight
                || floorSpriteTraits.Width != theWidth
                || floorSpriteTraits.Height != theHeight)
            {
                throw new Exception("Brick sprite sets aren't the same dimensions!");
            }

            var electricTiles = RecolourByThresholdAndColourWheel(
                new HostSuppliedSprite[]
                {
                    outlineSpriteTraits.GetHostImageObject(levelNumber % outlineSpriteTraits.ImageCount),
                    outlineSpriteTraits.GetHostImageObject((levelNumber + 1) % outlineSpriteTraits.ImageCount)
                },
                levelNumber * ElectricBrickLevelSeparation);

            var wallTiles = RecolourByThresholdAndColourWheel(
                new HostSuppliedSprite[]
                {
                    brickSpriteTraits.GetHostImageObject(levelNumber % brickSpriteTraits.ImageCount),
                    brickSpriteTraits.GetHostImageObject((levelNumber + 1) % brickSpriteTraits.ImageCount)
                },
                levelNumber * WallBrickLevelSeparation);

            var baseBrickGreyLevel = (levelNumber & 1) * FloorBrickLevelSeparation + FloorBrickLevelBase;

            var floorTiles = new HostSuppliedSprite[]
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

            var resultSprites = new HostSuppliedSprite[10]; // NB: Not all slots are needed, only those below:
            resultSprites[MissionIITile.FloorMask] = floorTiles[0];
            resultSprites[MissionIITile.FloorMask+1] = floorTiles[1];
            resultSprites[MissionIITile.WallMask] = wallTiles[0];
            resultSprites[MissionIITile.WallMask + 1] = wallTiles[1];
            resultSprites[MissionIITile.ElectricWallMask] = electricTiles[0];
            resultSprites[MissionIITile.ElectricWallMask + 1] = electricTiles[1];
            return resultSprites;
        }



        private static HostSuppliedSprite[] RecolourByThresholdAndColourWheel(HostSuppliedSprite[] hostSpritesArray, int seedValue)
        {
            var theList = new List<HostSuppliedSprite>();
            foreach(var hostSprite in hostSpritesArray)
            {
                var highColour = Colour.GetWheelColourAsPackedValue(seedValue);
                var lowColour = Colour.GetWheelColourAsPackedValue(seedValue + TwoColourBrickColourSeparation);
                var imageDataArray = hostSprite.PixelsToUintArray();
                Colour.ReplaceWithThreshold(imageDataArray, highColour, lowColour);
                var newHostImage = GameClassLibrary.Graphics.HostSuppliedSprite.UintArrayToSprite(imageDataArray, hostSprite.BoardWidth, hostSprite.BoardHeight);
                theList.Add(newHostImage);
                seedValue += ColourSeparationBetweenColouredBricks;
            }
            return theList.ToArray();
        }



        private static HostSuppliedSprite RecolourByThresholdAndGreyLevels(HostSuppliedSprite hostSprite, int lowGreyLevel, int highGreyLevel)
        {
            return RecolourByThresholdAndSpecificColours(
                hostSprite,
                Colour.ExpandToGreyScaleArgb((byte) highGreyLevel),
                Colour.ExpandToGreyScaleArgb((byte) lowGreyLevel));
        }



        private static HostSuppliedSprite RecolourByThresholdAndSpecificColours(HostSuppliedSprite hostSprite, uint highColour, uint lowColour)
        {
            var imageDataArray = hostSprite.PixelsToUintArray();
            Colour.ReplaceWithThreshold(imageDataArray, highColour, lowColour);
            return HostSuppliedSprite.UintArrayToSprite(imageDataArray, hostSprite.BoardWidth, hostSprite.BoardHeight);
        }
    }
}
