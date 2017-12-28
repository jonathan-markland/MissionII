using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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



        public static void UpdateTo(CybertronGameBoard gameBoard, CybertronKeyStates keyStates)
        {
            gameBoard.Man.AdvanceOneCycle(gameBoard, keyStates);

            foreach (var thisDroid in gameBoard.DroidsInRoom)
            {
                thisDroid.AdvanceOneCycle(gameBoard, keyStates);
            }

            gameBoard.Ghost.AdvanceOneCycle(gameBoard, keyStates);

            MoveBullets(gameBoard);

            foreach (var thisExplosion in gameBoard.ExplosionsInRoom)
            {
                thisExplosion.AdvanceOneCycle(gameBoard, keyStates);
            }

            foreach (var explosionToRemove in gameBoard.ExplosionsToRemove)
            {
                gameBoard.ExplosionsInRoom.Remove(explosionToRemove);
            }

            gameBoard.ExplosionsToRemove.Clear();
        }



        public static void IncrementScore(CybertronGameBoard gameBoard, int scoreDelta)
        {
            gameBoard.Score = (uint) (gameBoard.Score + scoreDelta);
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

            foreach(var theDroid in gameBoard.DroidsInRoom)
            {
                var droidRectangle = theDroid.GetBoundingRectangle();
                if (droidRectangle.Left != oldX || droidRectangle.Top != oldY) // TODO: crude way of avoiding self-intersection test
                {
                    if (droidRectangle.Intersects(myNewRectangle))
                    {
                        return CollisionDetection.WallHitTestResult.HitWall; // Pretend other monsters are wall.  Doesn't matter.
                    }
                }
            }

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

            cybertronGameBoard.BulletsInRoom.Add(
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



        public static void MoveBullets(CybertronGameBoard gameBoard)
        {
            if (gameBoard.BulletsInRoom.Count == 0) return; // optimisation
            for( int i=0; i < Constants.BulletCycles; i++)
            {
                MoveBulletsOnePixel(gameBoard);
            }
        }



        public static void MoveBulletsOnePixel(CybertronGameBoard gameBoard)
        {
            var n = gameBoard.BulletsInRoom.Count;
            for (int i=n-1; i >= 0; --i)
            {
                var theBullet = gameBoard.BulletsInRoom[i];
                var proposedX = theBullet.Sprite.RoomX + theBullet.BulletDirection.dx;
                var proposedY = theBullet.Sprite.RoomY + theBullet.BulletDirection.dy;

                var hitResult = CollisionDetection.HitsWalls(
                    gameBoard.CurrentRoomWallData,
                    CybertronGameBoardConstants.TileWidth,
                    CybertronGameBoardConstants.TileHeight,
                    proposedX,
                    proposedY,
                    theBullet.Sprite.Traits.BoardWidth,
                    theBullet.Sprite.Traits.BoardHeight);

                if (hitResult == CollisionDetection.WallHitTestResult.NothingHit)
                {
                    theBullet.Sprite.RoomX = proposedX;
                    theBullet.Sprite.RoomY = proposedY;

                    if (KillThingsIfShot(gameBoard, theBullet))
                    {
                        gameBoard.BulletsInRoom.RemoveAt(i);
                    }
                }
                else // Bullet hit wall or went outside room.
                {
                    gameBoard.BulletsInRoom.RemoveAt(i);
                }
            }
        }



        public static bool KillThingsIfShot(CybertronGameBoard gameBoard, CybertronBullet theBullet)
        {
            // TODO: We are NOT considering the dimensions of the bullet!
            // Only a single point, which is actually just the top left corner!

            if (theBullet.Sprite.Intersects(gameBoard.Man.SpriteInstance))
            {
                gameBoard.Man.YouHaveBeenShot(gameBoard);
                return true;
            }

            if (theBullet.Sprite.GetBoundingRectangle().Intersects(gameBoard.Ghost.GetBoundingRectangle())) // TODO: Allow man's bullets to stun only.
            {
                gameBoard.Ghost.YouHaveBeenShot(gameBoard);
                return true;
            }

            int n = gameBoard.DroidsInRoom.Count;
            for(int i = n-1; i >= 0; --i)
            {
                var thisDroid = gameBoard.DroidsInRoom[i];
                if (theBullet.Sprite.Intersects(thisDroid.SpriteInstance))
                {
                    thisDroid.YouHaveBeenShot(gameBoard);
                    System.Diagnostics.Debug.Assert(gameBoard.DroidsInRoom.Count == n); // CreateYourExplosion() must NOT invalidate the count!
                    gameBoard.DroidsInRoom.RemoveAt(i);
                    if (theBullet.IncreasesScore)
                    {
                        IncrementScore(gameBoard, CybertronGameBoardConstants.MonsterKillingScore);
                    }
                    return true;
                }
            }

            return false; // hit nothing.
        }



        public static void AddObjectIfInCurrentRoom(CybertronGameBoard theGameBoard, CybertronObject theObject)
        {
            if (theObject.RoomNumber == theGameBoard.RoomNumber)
            {
                theGameBoard.ObjectsInRoom.Add(theObject);
            }
        }



        public static void PrepareForNewRoom(CybertronGameBoard theGameBoard)
        {
            // The Man must already be positioned.

            var thisRoomNumber = theGameBoard.RoomNumber;

            theGameBoard.CurrentRoomWallData = 
                theGameBoard.TheWorldWallData
                    .Levels[theGameBoard.LevelNumber - 1]
                    .Rooms[thisRoomNumber - 1]
                    .WallData;

            theGameBoard.BulletsInRoom.Clear();
            theGameBoard.DroidsInRoom.Clear();
            theGameBoard.ObjectsInRoom.Clear();
            theGameBoard.ExplosionsInRoom.Clear();
            theGameBoard.ExplosionsToRemove.Clear();

            // Are any objects in this room?

            AddObjectIfInCurrentRoom(theGameBoard, theGameBoard.Key);
            AddObjectIfInCurrentRoom(theGameBoard, theGameBoard.Ring);
            AddObjectIfInCurrentRoom(theGameBoard, theGameBoard.Gold);
            // TODO:  AddObjectIfInCurrentRoom(theGameBoard, theGameBoard.Safe);

            // HACKS

            var pointsList = new List<Point>();

            var manRectangle = theGameBoard.Man.GetBoundingRectangle(); // Man should have been positioned by caller.

            int posnWidth = 20;
            int posnHeight = 20;

            PositionFinder.ForEachEmptyCell(
                theGameBoard.CurrentRoomWallData,
                CybertronGameBoardConstants.TileWidth,
                CybertronGameBoardConstants.TileHeight,
                posnWidth, // HACK : calculate width of widest sprite being positioned.
                posnHeight, // HACK : calculate height of tallest sprite being positioned.
                (x,y) =>
                {
                    if (!manRectangle.Intersects(new Rectangle(x, y, posnWidth, posnHeight)))
                    {
                        pointsList.Add(new Point(x, y));
                    }
                    return true;
                });

            Business.Shuffle(pointsList, RandomGenerator);

            // TODO: position keys etc too, where keys are priority.

            var droidPoints = pointsList.Take(8);
            var droidsList = new List<CybertronDroid>();

            foreach(var droidPoint in droidPoints)
            {
                var monsterType = (RandomGenerator.Next(10));

                var monsterGraphic =
                    (monsterType < 6)
                    ? GameClassLibrary.CybertronSpriteTraits.Monster1
                    : GameClassLibrary.CybertronSpriteTraits.Monster2;

                var monsterAI =
                    (monsterType < 6)
                        ? new ArtificialIntelligence.Attractor() as ArtificialIntelligence.AbstractIntelligenceProvider
                        : new ArtificialIntelligence.SingleMinded() as ArtificialIntelligence.AbstractIntelligenceProvider;

                droidsList.Add(
                    new CybertronDroid(
                        droidPoint.X, droidPoint.Y,
                        monsterGraphic,
                        monsterAI));
            }

            theGameBoard.DroidsInRoom = droidsList;

            theGameBoard.Ghost = new CybertronGhost();
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
