
using GameClassLibrary.Graphics;

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
        }
    }
}
