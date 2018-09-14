
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;
using GameClassLibrary.Walls;

namespace MissionIIClassLibrary
{
    public interface IGameBoard // TODO: Distill OUT of here MissionII specific stuff, before moving to library.
    {
        TileMatrix GetTileMatrix();

        void Add(GameObject o);
        void Remove(GameObject o);
        void ForEachObjectInPlayDo<A>(Action<A> theAction) where A : class;

        GameObject GetMan(); // TODO: inconsistent terms Man vs. Player
        void AddToPlayerInventory(Interactibles.InteractibleObject o);
        bool PlayerInventoryContains(Interactibles.InteractibleObject o);
        void PlayerIncrementScore(int deltaAmount);
        void PlayerGainLife();
        void PlayerLoseLife();
        // Missing drop from inventory
        // Missing multi-player

        void PrepareForNewLevel(int newLevelNumber);
        int GetLevelNumber();

        CollisionDetection.WallHitTestResult MoveManOnePixel(MovementDeltas movementDeltas);

        CollisionDetection.WallHitTestResult MoveAdversaryOnePixel(GameObject adversaryObject, MovementDeltas movementDeltas);
        // THe base class for adversaries would need to be in the library.

        void MoveRoomNumberByDelta(int roomNumberDelta);
        void PrepareForNewRoom();
        void ForEachThingWeHaveToFindOnThisLevel(Action<Interactibles.InteractibleObject> theAction);
        bool ManIsInvincible();
        void ManGainInvincibility();
    }



    public static class IGameBoardExtensionsForMissionII
    {
        public static void Electrocute(this IGameBoard gameBoard, ElectrocutionMethod electrocutionMethod)
        {
            ((MissionIIGameBoard)gameBoard).Man.Electrocute(electrocutionMethod);
        }



        /// <summary>
        /// Returns true if any droids exist in the room.
        /// </summary>
        public static bool DroidsExistInRoom(this IGameBoard gameBoard)
        {
            bool foundDroids = false;
            gameBoard.ForEachObjectInPlayDo<Droids.BaseDroid>(o =>
            {
                foundDroids = true;   // TODO: Library issue:  It's not optimal that we can't break the ForEach.
            });
            return foundDroids;
        }



        public static GameClassLibrary.Math.Point GetCornerFurthestAwayFromMan(this IGameBoard gameBoard)
        {
            var cx = Constants.ScreenWidth / 2;
            var cy = Constants.ScreenHeight / 2;
            var manCentre = gameBoard.GetMan().GetBoundingRectangle().Centre;
            var x = manCentre.X < cx ? Constants.ScreenWidth : 0;
            var y = manCentre.Y < cy ? Constants.ScreenHeight : 0;
            return new Point(x, y);
        }



        public struct BulletResult
        {
            public uint HitCount;
            public int TotalScoreIncrease;
        }



        public static BulletResult KillThingsIfShotAndGetHitCount(
            this IGameBoard gameBoard,
            GameObjects.Bullet theBullet)
        {
            int scoreDelta = 0;
            uint hitCount = 0;

            gameBoard.ForEachObjectInPlayDo<GameObject>(o =>
            {
                if (o.GetBoundingRectangle().Intersects(theBullet.GetBoundingRectangle()))
                {
                    var shotResult = o.YouHaveBeenShot(gameBoard, theBullet.IncreasesScore);
                    if (shotResult.Affirmed)
                    {
                        if (theBullet.IncreasesScore)
                        {
                            scoreDelta += shotResult.ScoreIncrease;
                        }
                        ++hitCount;
                    }
                }
            });

            return new BulletResult { HitCount = hitCount, TotalScoreIncrease = scoreDelta };
        }



        public static void StartBullet(
            this IGameBoard gameBoard,
            Rectangle gameObjectextentsRectangle,
            MovementDeltas bulletDirection,
            bool increasesScore)
        {
            // TODO: Separate out a bit for unit testing?

            var r = gameObjectextentsRectangle; // convenience!

            if (increasesScore)
            {
                MissionIISounds.ManFiring.Play();
            }
            else
            {
                MissionIISounds.DroidFiring.Play();
            }

            var theBulletTraits = MissionIISprites.Bullet;
            var bulletWidth = theBulletTraits.Width;
            var bulletHeight = theBulletTraits.Height;

            int x, y;

            if (bulletDirection.dx < 0)
            {
                x = (r.Left - bulletWidth) - Constants.BulletSpacing;
            }
            else if (bulletDirection.dx > 0)
            {
                x = r.Left + r.Width + Constants.BulletSpacing;
            }
            else // (bulletDirection.dx == 0)
            {
                x = r.Left + ((r.Width - bulletWidth) / 2);
            }

            if (bulletDirection.dy < 0)
            {
                y = (r.Top - bulletHeight) - Constants.BulletSpacing;
            }
            else if (bulletDirection.dy > 0)
            {
                y = r.Top + r.Height + Constants.BulletSpacing;
            }
            else // (bulletDirection.dy == 0)
            {
                y = r.Top + ((r.Height - bulletHeight) / 2);
            }

            if (bulletDirection.dx == 0 && bulletDirection.dy == 0)
            {
                return;  // Cannot ascertain a direction away from the source sprite, so do nothing.
            }

            gameBoard.Add(
                new GameObjects.Bullet
                (
                    new SpriteInstance
                    {
                        X = x,
                        Y = y,
                        Traits = theBulletTraits
                    }
                    , bulletDirection
                    , increasesScore
                ));
        }

    }


    public struct ShotStruct
    {
        public bool Affirmed;
        public int ScoreIncrease;
    }


    public static class GameObjectExtensions // TODO: move back into the librarry
    {
        public static void MoveBy(this GameObject gameObject, MovementDeltas movementDeltas)
        {
            var p = gameObject.TopLeftPosition;
            gameObject.TopLeftPosition = new Point(p.X + movementDeltas.dx, p.Y + movementDeltas.dy);
        }

        public static bool Intersects(this GameObject gameObject, GameObject otherObject)
        {
            return gameObject.GetBoundingRectangle().Intersects(otherObject.GetBoundingRectangle());
        }

        /// <summary>
        /// It is advised that the movement is by ONE pixel at a time.
        /// </summary>
        public static CollisionDetection.WallHitTestResult MoveConsideringWallsOnly(
            this GameObject gameObject,
            TileMatrix wallMatrix,
            MovementDeltas movementDeltas,
            Func<Tile, bool> isFloorFunc)
        {
            var r = gameObject.GetBoundingRectangle();
            var proposedX = r.Left + movementDeltas.dx;
            var proposedY = r.Top + movementDeltas.dy;

            // First consider both X and Y deltas directly:

            var hitResult = CollisionDetection.HitsWalls(
                wallMatrix,
                proposedX, proposedY,
                r.Width,
                r.Height,
                isFloorFunc);

            if (hitResult == CollisionDetection.WallHitTestResult.NothingHit)
            {
                gameObject.TopLeftPosition = new Point(proposedX, proposedY);
            }

            return hitResult;
        }



        public static MovementDeltas GetMovementDeltasToHeadTowards(
            this GameObject aggressorSprite,
            GameObject targetSprite)
        {
            var targetCentre = targetSprite.GetBoundingRectangle().Centre;
            var aggressorCentre = aggressorSprite.GetBoundingRectangle().Centre;

            int dx = 0;
            if (targetCentre.X < aggressorCentre.X) dx = -1;
            if (targetCentre.X > aggressorCentre.X) dx = 1;

            int dy = 0;
            if (targetCentre.Y < aggressorCentre.Y) dy = -1;
            if (targetCentre.Y > aggressorCentre.Y) dy = 1;

            return new MovementDeltas(dx, dy);
        }
    }



    public abstract class GameObject // TODO: Move into library AFTER IGameBoard is distilled
    {
        // Note - We avoid restricting a GameObject to be a *single* SpriteInstance.

            // TODO: Can these be abstract properties?
        public abstract Rectangle GetBoundingRectangle();
        public abstract Point TopLeftPosition { get; set; }
        public abstract bool CanBeOverlapped { get; }
        public abstract void AdvanceOneCycle(IGameBoard theGameBoard, KeyStates theKeyStates);
        public abstract void Draw(IDrawingTarget drawingTarget);
        public abstract void ManWalkedIntoYou(IGameBoard theGameBoard);
        public abstract ShotStruct YouHaveBeenShot(IGameBoard theGameBoard, bool shotByMan);
        public virtual int KillScore { get { return 0; } }
        public virtual int CollectionScore { get { return 0; } }
    }
}
