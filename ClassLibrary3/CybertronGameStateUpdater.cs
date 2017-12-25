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
    }



    public static class CybertronGameStateUpdater
    {
        public static void UpdateTo(GameTimeSpan timeSinceGameStart, CybertronGameBoard gameBoard, CybertronKeyStates keyStates)
        {
            // TODO: Not yet using timeSinceGameStart properly (clock the following several times as needed):

            gameBoard.Man.AdvanceOneCycle(gameBoard, keyStates);

            foreach (var thisDroid in gameBoard.DroidsInRoom)
            {
                thisDroid.AdvanceOneCycle(gameBoard, keyStates);
            }

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
            var proposedManX = gameBoard.Man.SpriteInstance.RoomX + movementDeltas.dx;
            var proposedManY = gameBoard.Man.SpriteInstance.RoomY + movementDeltas.dy;

            var hitResult = CollisionDetection.HitsWalls(
                    gameBoard.CurrentRoomWallData,
                    CybertronGameBoardConstants.TileWidth,
                    CybertronGameBoardConstants.TileHeight,
                    proposedManX,
                    proposedManY,
                    gameBoard.Man.SpriteInstance.Traits.BoardWidth,
                    gameBoard.Man.SpriteInstance.Traits.BoardHeight);

            if (hitResult == CollisionDetection.WallHitTestResult.NothingHit)
            {
                gameBoard.Man.SpriteInstance.RoomX = proposedManX;
                gameBoard.Man.SpriteInstance.RoomY = proposedManY;
            }

            return hitResult;
        }



        public static void StartBullet(
            SpriteInstance sourceSprite, 
            int facingDirection,
            CybertronGameBoard cybertronGameBoard)
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
                ));
        }



        public static void MoveBullets(CybertronGameBoard gameBoard)
        {
            if (gameBoard.BulletsInRoom.Count == 0) return; // optimisation
            for( int i=0; i<3; i++)
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
                    if (KillThingsIfShot(gameBoard, proposedX, proposedY))
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



        public static bool KillThingsIfShot(CybertronGameBoard gameBoard, int bulletX, int bulletY)
        {
            // TODO: We are NOT considering the dimensions of the bullet!
            // Only a single point, which is actually just the top left corner!

            if (Intersects(bulletX, bulletY, gameBoard.Man.SpriteInstance))
            {
                gameBoard.Man.Die();
                return true;
            }

            int n = gameBoard.DroidsInRoom.Count;
            for(int i=n-1; i>=0; --i)
            {
                var thisDroid = gameBoard.DroidsInRoom[i];
                if (Intersects(bulletX, bulletY, thisDroid.SpriteInstance))
                {
                    thisDroid.CreateYourExplosion(gameBoard);
                    System.Diagnostics.Debug.Assert(gameBoard.DroidsInRoom.Count == n); // CreateYourExplosion() must NOT invalidate the count!
                    gameBoard.DroidsInRoom.RemoveAt(i);
                    IncrementScore(gameBoard, CybertronGameBoardConstants.MonsterKillingScore);
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

            var randomNum = new Random(); // TODO: move out
            Business.Shuffle(pointsList, randomNum);

            // TODO: position keys etc too, where keys are priority.

            var droidsList = pointsList.Take(5).Select(
                o => new CybertronDroid(o.X, o.Y, GameClassLibrary.CybertronSpriteTraits.Monster1)).ToList();

            theGameBoard.DroidsInRoom = droidsList;
        }



        public static bool Intersects(int bulletX, int bulletY, SpriteInstance spriteInstance)
        {
            // TODO: We are NOT considering the dimensions of the bullet!
            if (bulletX < spriteInstance.RoomX) return false;
            if (bulletY < spriteInstance.RoomY) return false;
            if (bulletX >= (spriteInstance.RoomX + spriteInstance.Traits.BoardWidth)) return false;
            if (bulletY >= (spriteInstance.RoomY + spriteInstance.Traits.BoardWidth)) return false;
            return true;
        }
    }
}
