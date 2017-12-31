using System;
using System.Collections.Generic;
using System.Linq;

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
            gameBoard.AllowForEachDo();

            gameBoard.ForEachDo(o => { o.AdvanceOneCycle(gameBoard, keyStates); return true; } );

            foreach (var objectToRemove in gameBoard.ObjectsToRemove)
            {
                gameBoard.ObjectsInRoom.Remove(objectToRemove);
            }

            gameBoard.ObjectsToRemove.Clear();
        }



        public static void IncrementScore(CybertronGameBoard gameBoard, int scoreDelta)
        {
            var thresholdBefore = gameBoard.Score / Constants.NewLifeBoundary;
            gameBoard.Score = (uint) (gameBoard.Score + scoreDelta);
            var thresholdAfter = gameBoard.Score / Constants.NewLifeBoundary;
            if (thresholdBefore < thresholdAfter)
            {
                if (gameBoard.Lives < Constants.MaxLives)
                {
                    ++gameBoard.Lives;
                }
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

            foreach(var theObject in gameBoard.ObjectsInRoom)
            {
                if (theObject.IsSolid)
                {
                    var objectRectangle = theObject.GetBoundingRectangle();
                    if (objectRectangle.Left != oldX || objectRectangle.Top != oldY) // TODO: crude way of avoiding self-intersection test
                    {
                        if (objectRectangle.Intersects(myNewRectangle))
                        {
                            return CollisionDetection.WallHitTestResult.HitWall; // Pretend other objects are wall.  Doesn't matter.
                        }
                    }
                }
            }

            // TODO: remove when man is in the objects list:
            if (myNewRectangle.Intersects(gameBoard.Man.SpriteInstance.GetBoundingRectangle()))
            {
                return CollisionDetection.WallHitTestResult.HitWall; // Pretend man is wall.  Doesn't matter.
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
            // TODO:  Do with polymorphism as much as possible.  Implies Man objects should be in ObjectsInRoom collection.

            bool hitSomething = false;

            gameBoard.ForEachDo(o => 
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
                return true;
            });

            return hitSomething;
        }



        public static void AddObjectIfInCurrentRoom(CybertronGameBoard theGameBoard, CybertronObject theObject)
        {
            if (theObject.RoomNumber == theGameBoard.RoomNumber)
            {
                theGameBoard.ObjectsInRoom.Add(theObject);
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
                // TODO: sort out game over
            }
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

            theGameBoard.CurrentRoomWallData = 
                theGameBoard.TheWorldWallData
                    .Levels[theGameBoard.LevelNumber - 1]
                    .Rooms[thisRoomNumber - 1]
                    .WallData;

            theGameBoard.ObjectsInRoom.Clear();
            theGameBoard.ObjectsToRemove.Clear();

            // Man should have been positioned by caller.
            // Establish an exclusion zone around the man so that nothing
            // is positioned too close to him.

            var exclusionRectangle = 
                theGameBoard.Man.GetBoundingRectangle()
                .Inflate(Constants.ExclusionZoneAroundMan);

            // Make a list of those things that need positioning.

            AddObjectIfInCurrentRoom(theGameBoard, theGameBoard.Key);
            AddObjectIfInCurrentRoom(theGameBoard, theGameBoard.Ring);
            AddObjectIfInCurrentRoom(theGameBoard, theGameBoard.Gold);
            AddObjectIfInCurrentRoom(theGameBoard, theGameBoard.Safe);

            for (int j=0; j<Constants.IdealDroidCountPerRoom; j++)
            {
                var monsterType = (RandomGenerator.Next(10));
                theGameBoard.ObjectsInRoom.Add(
                    (monsterType < 6)
                        ? new CybertronRedDroid() as CybertronDroidBase
                        : new CybertronBlueDroid() as CybertronDroidBase);
            }

            // Now measure the max dimensions of the things that need measuring.

            int posnWidth = Constants.PositionerShapeSizeMinium;
            int posnHeight = Constants.PositionerShapeSizeMinium;

            foreach (var obj in theGameBoard.ObjectsInRoom)
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
                if (i >= theGameBoard.ObjectsInRoom.Count) break;
                theGameBoard.ObjectsInRoom[i].TopLeftPosition = pointsList[i];
            }
            if (i < theGameBoard.ObjectsInRoom.Count)
            {
                theGameBoard.ObjectsInRoom.RemoveRange(i, theGameBoard.ObjectsInRoom.Count - i);
            }

            // Add other objects to the list, that don't require the positioner:

            theGameBoard.ObjectsInRoom.Add(new CybertronGhost());
            theGameBoard.ObjectsInRoom.Add(theGameBoard.Man);
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
