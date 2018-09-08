
using System;
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
            SpriteTraits floorSpriteTraits,
            uint argbColourForElectricTiles)
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

            var electricTile1 =
                electricSpriteTraits.GetHostImageObject(levelNumber % electricSpriteTraits.ImageCount)
                    .RecolourByThreshold(
                        argbColourForElectricTiles,
                        argbColourForElectricTiles);

            var electricTile2 =
                electricSpriteTraits.GetHostImageObject((levelNumber + 1) % electricSpriteTraits.ImageCount)
                    .RecolourByThreshold(
                        argbColourForElectricTiles,
                        argbColourForElectricTiles);

            var nw1 = levelNumber * WallBrickLevelSeparation;

            var wallTile1 =
                wallSpriteTraits.GetHostImageObject(levelNumber % wallSpriteTraits.ImageCount)
                    .RecolourByThresholdAndColourWheel(
                        nw1,
                        nw1 + TwoColourBrickColourSeparation);

            var nw2 = nw1 + ColourSeparationBetweenColouredBricks;

            var wallTile2 =
                wallSpriteTraits.GetHostImageObject((levelNumber + 1) % wallSpriteTraits.ImageCount)
                    .RecolourByThresholdAndColourWheel(
                        nw2,
                        nw2 + TwoColourBrickColourSeparation);

            var baseBrickGreyLevel = (levelNumber & 1) * FloorBrickLevelSeparation + FloorBrickLevelBase;

            var floorTile1 =
                floorSpriteTraits.GetHostImageObject(levelNumber % floorSpriteTraits.ImageCount)
                    .RecolourByThresholdAndGreyLevels(
                        baseBrickGreyLevel,
                        baseBrickGreyLevel + GreyLevelSeparation);

            var floorTile2 =
                floorSpriteTraits.GetHostImageObject((levelNumber + 1) % floorSpriteTraits.ImageCount)
                    .RecolourByThresholdAndGreyLevels(
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
    }
}
