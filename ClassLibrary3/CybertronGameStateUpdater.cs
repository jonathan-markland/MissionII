using System;
using System.Collections.Generic;

namespace GameClassLibrary
{
    public struct MovementDeltas
    {
        public MovementDeltas(int dx, int dy) { this.dx = dx; this.dy = dy; }
        public int dx;
        public int dy;

        public bool Stationary { get { return dx == 0 && dy == 0; } }
    }



    public static class CybertronGameStateUpdater
    {
        public static Random RandomGenerator = new Random();



        public static void Update(CybertronGameBoard gameBoard, CybertronKeyStates keyStates)
        {
            gameBoard.ObjectsInRoom.ForEachDo(o => { o.AdvanceOneCycle(gameBoard, keyStates); } );
            gameBoard.ObjectsInRoom.RemoveThese(gameBoard.ObjectsToRemove);
            gameBoard.ObjectsToRemove.Clear();
        }



        public static void IncrementScore(CybertronGameBoard gameBoard, int scoreDelta)
        {
            var thresholdBefore = gameBoard.Score / Constants.NewLifeBoundary;
            gameBoard.Score = (uint) (gameBoard.Score + scoreDelta);
            var thresholdAfter = gameBoard.Score / Constants.NewLifeBoundary;
            if (thresholdBefore < thresholdAfter)
            {
                IncrementLives(gameBoard);
            }
        }



        public static void IncrementLives(CybertronGameBoard gameBoard)
        {
            if (gameBoard.Lives < Constants.MaxLives)
            {
                ++gameBoard.Lives;
            }
        }



        public static CollisionDetection.WallHitTestResult MoveManOnePixel(CybertronGameBoard gameBoard, MovementDeltas movementDeltas)
        {
            return MoveSpriteInstanceOnePixel(
                gameBoard.CurrentRoomWallData,
                gameBoard.Man.SpriteInstance, 
                movementDeltas);
        }



        public static CollisionDetection.WallHitTestResult MoveAdversaryOnePixel(
            CybertronGameBoard gameBoard, 
            SpriteInstance spriteInstance, 
            MovementDeltas movementDeltas)
        {
            var oldX = spriteInstance.RoomX;
            var oldY = spriteInstance.RoomY;

            var myNewRectangle = new Rectangle(
                oldX + movementDeltas.dx, 
                oldY + movementDeltas.dy, 
                spriteInstance.Traits.BoardWidth, 
                spriteInstance.Traits.BoardHeight);

            var hitResult = CollisionDetection.WallHitTestResult.NothingHit;
            gameBoard.ObjectsInRoom.ForEachDo(theObject =>
            {
                if (hitResult == CollisionDetection.WallHitTestResult.NothingHit
                    && theObject.IsSolid)
                {
                    var objectRectangle = theObject.GetBoundingRectangle();
                    if (objectRectangle.Left != oldX || objectRectangle.Top != oldY) // TODO: crude way of avoiding self-intersection test
                    {
                        if (objectRectangle.Intersects(myNewRectangle))
                        {
                            hitResult = CollisionDetection.WallHitTestResult.HitWall; // Pretend other objects are wall.  Doesn't matter.
                        }
                    }
                }
            });

            if (hitResult != CollisionDetection.WallHitTestResult.NothingHit)
            {
                return hitResult;
            }

            return MoveSpriteInstanceOnePixel(
                gameBoard.CurrentRoomWallData,
                spriteInstance,
                movementDeltas);
        }



        public static void MoveAdversaryOnePixelUnchecked(SpriteInstance spriteInstance, MovementDeltas movementDeltas)
        {
            spriteInstance.RoomX = spriteInstance.RoomX + movementDeltas.dx;
            spriteInstance.RoomY = spriteInstance.RoomY + movementDeltas.dy;
        }



        public static CollisionDetection.WallHitTestResult MoveSpriteInstanceOnePixel(WallMatrix wallMatrix, SpriteInstance spriteInstance, MovementDeltas movementDeltas)
        {
            var proposedX = spriteInstance.RoomX + movementDeltas.dx;
            var proposedY = spriteInstance.RoomY + movementDeltas.dy;

            var hitResult = CollisionDetection.HitsWalls(
                    wallMatrix,
                    CybertronGameBoardConstants.TileWidth,
                    CybertronGameBoardConstants.TileHeight,
                    proposedX,
                    proposedY,
                    spriteInstance.Traits.BoardWidth,
                    spriteInstance.Traits.BoardHeight);

            if (hitResult == CollisionDetection.WallHitTestResult.NothingHit)
            {
                spriteInstance.RoomX = proposedX;
                spriteInstance.RoomY = proposedY;
            }

            return hitResult;
        }



        public static void StartBullet(
            SpriteInstance sourceSprite, 
            int facingDirection,
            CybertronGameBoard cybertronGameBoard,
            bool increasesScore)
        {
            // TODO: Separate out a bit for unit testing?

            var bulletDirection = Business.GetMovementDeltas(facingDirection);
            var theBulletTraits = CybertronSpriteTraits.Bullet;
            var bulletWidth = theBulletTraits.BoardWidth;
            var bulletHeight = theBulletTraits.BoardHeight;

            int x, y;

            if (bulletDirection.dx < 0)
            {
                x = (sourceSprite.RoomX - bulletWidth) - Constants.BulletSpacing;
            }
            else if (bulletDirection.dx > 0)
            {
                x = sourceSprite.RoomX + sourceSprite.Traits.BoardWidth + Constants.BulletSpacing;
            }
            else // (bulletDirection.dx == 0)
            {
                x = sourceSprite.RoomX + ((sourceSprite.Traits.BoardWidth - bulletWidth) / 2);
            }

            if (bulletDirection.dy < 0)
            {
                y = (sourceSprite.RoomY - bulletHeight) - Constants.BulletSpacing;
            }
            else if (bulletDirection.dy > 0)
            {
                y = sourceSprite.RoomY + sourceSprite.Traits.BoardHeight + Constants.BulletSpacing;
            }
            else // (bulletDirection.dy == 0)
            {
                y = sourceSprite.RoomY + ((sourceSprite.Traits.BoardHeight - bulletHeight) / 2);
            }

            if (bulletDirection.dx == 0 && bulletDirection.dy == 0)
            {
                return;  // Cannot ascertain a direction away from the source sprite, so do nothing.
            }

            cybertronGameBoard.ObjectsInRoom.Add(
                new CybertronBullet
                (
                    new SpriteInstance
                    {
                        RoomX = x,
                        RoomY = y,
                        Traits = theBulletTraits
                    }
                    , bulletDirection
                    , increasesScore
                ));
        }



        public static bool KillThingsIfShot(CybertronGameBoard gameBoard, CybertronBullet theBullet)
        {
            bool hitSomething = false;

            gameBoard.ObjectsInRoom.ForEachDo(o => 
            {
                if (o.GetBoundingRectangle().Intersects(theBullet.GetBoundingRectangle()))
                {
                    if (o.YouHaveBeenShot(gameBoard))
                    {
                        if (theBullet.IncreasesScore) // TODO: Code not quite the same as before.  Score increment to move into target classes.
                        {
                            IncrementScore(gameBoard, CybertronGameBoardConstants.MonsterKillingScore);
                        }
                        hitSomething = true;
                    }
                }
            });

            return hitSomething;
        }



        public static void AddObjectIfInCurrentRoom(CybertronGameBoard theGameBoard, CybertronObject theObject, List<CybertronGameObject> targetList)
        {
            if (theObject.RoomNumber == theGameBoard.RoomNumber)
            {
                targetList.Add(theObject);
            }
        }



        public static void LoseLife(CybertronGameBoard theGameBoard)
        {
            if (theGameBoard.Lives > 0)
            {
                --theGameBoard.Lives;
                RestartCurrentRoom(theGameBoard);
            }
            else
            {
                CybertronGameModeSelector.ModeSelector.CurrentMode = new CybertronGameOverMode();
            }
        }


        public static void PrepareForNewLevel(CybertronGameBoard theGameBoard)
        {
            // The LevelNumber is already set.

            // Determine this level object:
            var theLevel = theGameBoard.TheWorldWallData.Levels[theGameBoard.LevelNumber - 1];

            // Set the start room number:
            theGameBoard.RoomNumber = theLevel.ManStartRoom.RoomNumber;

            // Calculate size of cluster in pixels:
            var clusterSizeX = CybertronGameBoardConstants.TileWidth * Constants.DestClusterSide;
            var clusterSizeY = CybertronGameBoardConstants.TileHeight * Constants.DestClusterSide;

            // TODO: all man sprites must be checked for SAME dimensions.
            // Calculate centering offsets for man within cluster:
            var manCX = (clusterSizeX - CybertronSpriteTraits.ManStanding[0].BoardWidth) / 2;
            var manCY = (clusterSizeY - CybertronSpriteTraits.ManStanding[0].BoardHeight) / 2;

            // Set man start position (according to 'x' in source level data for this level):
            var manX = manCX + theLevel.ManStartCluster.X * clusterSizeX;
            var manY = manCY + theLevel.ManStartCluster.Y * clusterSizeY;
            theGameBoard.Man.Alive(0, manX, manY);

            // Clear inventory at start of each level.
            theGameBoard.PlayerInventory = new List<CybertronObject>();

            // TODO: This could be done better, as it's a bit weird requiring the objects already to
            //       be created, and then only to replace them.  At least this way we have ONE
            //       place that decides what is to be found on the level (ForEachThingWeHaveToFindOnThisLevel)
            theGameBoard.Key = new GameClassLibrary.CybertronKey(0);
            theGameBoard.Ring = new GameClassLibrary.CybertronRing(0);
            theGameBoard.Gold = new GameClassLibrary.CybertronGold(0);

            var roomNumberAllocator = new UniqueNumberAllocator(1, Constants.NumRooms);
            // var roomNumberAllocator = new IncrementingNumberAllocator(1, Constants.NumRooms); // For testing purposes.

            theGameBoard.ForEachThingWeHaveToFindOnThisLevel(o =>
            {
                var roomNumber = roomNumberAllocator.Next();
                if (o is CybertronKey)
                {
                    theGameBoard.Key = new GameClassLibrary.CybertronKey(roomNumber);
                }
                else if (o is CybertronRing)
                {
                    theGameBoard.Ring = new GameClassLibrary.CybertronRing(roomNumber);
                }
                else if (o is CybertronGold)
                {
                    theGameBoard.Gold = new GameClassLibrary.CybertronGold(roomNumber);
                }
                else
                {
                    System.Diagnostics.Debug.Assert(false); // What have we been passed ???
                }
            });

            theGameBoard.Safe = new GameClassLibrary.CybertronLevelSafe(roomNumberAllocator.Next());
            theGameBoard.Potion = new GameClassLibrary.CybertronPotion(roomNumberAllocator.Next());

            PrepareForNewRoom(theGameBoard);

            CybertronGameModeSelector.ModeSelector.CurrentMode = new CybertronEnteringLevelMode(theGameBoard);
        }



        public static void RestartCurrentRoom(CybertronGameBoard theGameBoard)
        {
            theGameBoard.Man.Position = theGameBoard.ManPositionOnRoomEntry;
            PrepareForNewRoom(theGameBoard);
        }



        public static void PrepareForNewRoom(CybertronGameBoard theGameBoard)
        {
            // The Man must already be positioned.

            // Remember initial position of man in case of loss of life:
            theGameBoard.ManPositionOnRoomEntry = theGameBoard.Man.Position;

            var thisRoomNumber = theGameBoard.RoomNumber;

            var maxLevelNumber = theGameBoard.TheWorldWallData.Levels.Count;

            theGameBoard.CurrentRoomWallData = 
                theGameBoard.TheWorldWallData
                    .Levels[(theGameBoard.LevelNumber - 1) % maxLevelNumber]
                    .Rooms[thisRoomNumber - 1]
                    .WallData;

            var objectsList = new List<CybertronGameObject>(); 

            theGameBoard.ObjectsToRemove.Clear();

            // Man should have been positioned by caller.
            // Establish an exclusion zone around the man so that nothing
            // is positioned too close to him.

            var exclusionRectangle = 
                theGameBoard.Man.GetBoundingRectangle()
                .Inflate(Constants.ExclusionZoneAroundMan);

            // Make a list of those things that need positioning.

            AddObjectIfInCurrentRoom(theGameBoard, theGameBoard.Key, objectsList);
            AddObjectIfInCurrentRoom(theGameBoard, theGameBoard.Ring, objectsList);
            AddObjectIfInCurrentRoom(theGameBoard, theGameBoard.Gold, objectsList);
            AddObjectIfInCurrentRoom(theGameBoard, theGameBoard.Safe, objectsList);
            AddObjectIfInCurrentRoom(theGameBoard, theGameBoard.Potion, objectsList);

            int redBlueThreshold = Constants.IdealDroidCountPerRoom;
            if (theGameBoard.LevelNumber == 2)
            {
                redBlueThreshold = Constants.IdealDroidCountPerRoom - 2;
            }
            else if (theGameBoard.LevelNumber > 2)
            {
                redBlueThreshold = Constants.IdealDroidCountPerRoom - 4;
            }

            for (int j=0; j < Constants.IdealDroidCountPerRoom; j++)
            {
                objectsList.Add(
                    (j < redBlueThreshold)
                        ? new CybertronRedDroid() as CybertronDroidBase
                        : new CybertronBlueDroid() as CybertronDroidBase);
            }

            // Now measure the max dimensions of the things that need measuring.

            int posnWidth = Constants.PositionerShapeSizeMinium;
            int posnHeight = Constants.PositionerShapeSizeMinium;

            foreach(var obj in objectsList)
            {
                var objRect = obj.GetBoundingRectangle();
                posnWidth = Math.Max(objRect.Width, posnWidth);
                posnHeight = Math.Max(objRect.Height, posnHeight);
            }

            // Now find a list of points on which we can position objects.

            var pointsList = new List<Point>();

            PositionFinder.ForEachEmptyCell(
                theGameBoard.CurrentRoomWallData,
                CybertronGameBoardConstants.TileWidth,
                CybertronGameBoardConstants.TileHeight,
                posnWidth, 
                posnHeight, 
                (x,y) =>
                {
                    if (!exclusionRectangle.Intersects(new Rectangle(x, y, posnWidth, posnHeight)))
                    {
                        pointsList.Add(new Point(x, y));
                    }
                    return true;
                });

            Business.Shuffle(pointsList, RandomGenerator);

            // Apply positions:
            // If this is LESS THAN ObjectsInRoom.Count then we cull the ObjectsInRoom container.

            int i = 0;
            for (; i<pointsList.Count; i++)
            {
                if (i >= objectsList.Count) break;
                objectsList[i].TopLeftPosition = pointsList[i];
            }
            if (i < objectsList.Count)
            {
                objectsList.RemoveRange(i, objectsList.Count - i);
            }

            // Add other objects to the list, that don't require the positioner:

            objectsList.Add(new CybertronGhost());
            objectsList.Add(theGameBoard.Man);

            theGameBoard.ObjectsInRoom.ReplaceWith(objectsList);
        }



        public static MovementDeltas GetMovementDeltasToHeadTowards(SpriteInstance aggressorSprite, SpriteInstance targetSprite)
        {
            var targetCentre = targetSprite.Centre;
            var aggressorCentre = aggressorSprite.Centre;

            int dx = 0;
            if (targetCentre.X < aggressorCentre.X) dx = -1;
            if (targetCentre.X > aggressorCentre.X) dx = 1;

            int dy = 0;
            if (targetCentre.Y < aggressorCentre.Y) dy = -1;
            if (targetCentre.Y > aggressorCentre.Y) dy = 1;

            return new MovementDeltas(dx, dy);
        }


    }
}
