
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
            SpriteTraits electricSpriteTraits,
            SpriteTraits wallSpriteTraits,
            SpriteTraits floorSpriteTraits)
        {
            --levelNumber; // because it's 1-based!

            var theWidth = electricSpriteTraits.Width;
            var theHeight = electricSpriteTraits.Height;

            if (wallSpriteTraits.Width != theWidth
                || wallSpriteTraits.Height != theHeight
                || floorSpriteTraits.Width != theWidth
                || floorSpriteTraits.Height != theHeight)
            {
                throw new Exception("Brick sprite sets aren't the same dimensions!");
            }

            var ne1 = levelNumber * ElectricBrickLevelSeparation;

            var electricTile1 = RecolourByThresholdAndColourWheel(
                electricSpriteTraits.GetHostImageObject(levelNumber % electricSpriteTraits.ImageCount),
                ne1,
                ne1 + TwoColourBrickColourSeparation);

            var ne2 = ne1 + ColourSeparationBetweenColouredBricks;

            var electricTile2 = RecolourByThresholdAndColourWheel(
                electricSpriteTraits.GetHostImageObject((levelNumber + 1) % electricSpriteTraits.ImageCount),
                ne2,
                ne2 + TwoColourBrickColourSeparation);

            var nw1 = levelNumber * WallBrickLevelSeparation;

            var wallTile1 = RecolourByThresholdAndColourWheel(
                wallSpriteTraits.GetHostImageObject(levelNumber % wallSpriteTraits.ImageCount),
                nw1,
                nw1 + TwoColourBrickColourSeparation);

            var nw2 = nw1 + ColourSeparationBetweenColouredBricks;

            var wallTile2 = RecolourByThresholdAndColourWheel(
                wallSpriteTraits.GetHostImageObject((levelNumber + 1) % wallSpriteTraits.ImageCount),
                nw2,
                nw2 + TwoColourBrickColourSeparation);

            var baseBrickGreyLevel = (levelNumber & 1) * FloorBrickLevelSeparation + FloorBrickLevelBase;

            var floorTile1 = RecolourByThresholdAndGreyLevels(
                floorSpriteTraits.GetHostImageObject(levelNumber % floorSpriteTraits.ImageCount),
                baseBrickGreyLevel,
                baseBrickGreyLevel + GreyLevelSeparation);

            var floorTile2 = RecolourByThresholdAndGreyLevels(
                floorSpriteTraits.GetHostImageObject((levelNumber + 1) % floorSpriteTraits.ImageCount),
                baseBrickGreyLevel + GreyLevelSeparation * 3,
                baseBrickGreyLevel + GreyLevelSeparation * 4);

            var resultSprites = new HostSuppliedSprite[10]; // NB: Not all slots are needed, only those below:
            resultSprites[MissionIITile.FloorMask] = floorTile1;
            resultSprites[MissionIITile.FloorMask+1] = floorTile2;
            resultSprites[MissionIITile.WallMask] = wallTile1;
            resultSprites[MissionIITile.WallMask + 1] = wallTile2;
            resultSprites[MissionIITile.ElectricWallMask] = electricTile1;
            resultSprites[MissionIITile.ElectricWallMask + 1] = electricTile2;
            return resultSprites;
        }



        private static HostSuppliedSprite RecolourByThresholdAndColourWheel(
            HostSuppliedSprite hostSprite, 
            int highColourSeed, 
            int lowColourSeed)
        {
            var highColour = Colour.GetWheelColourAsPackedValue(highColourSeed);
            var lowColour = Colour.GetWheelColourAsPackedValue(lowColourSeed);
            var imageDataArray = hostSprite.PixelsToUintArray();

            Colour.ReplaceWithThreshold(imageDataArray, highColour, lowColour);

            return GameClassLibrary.Graphics.HostSuppliedSprite.UintArrayToSprite(
                imageDataArray, 
                hostSprite.BoardWidth, 
                hostSprite.BoardHeight);
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
