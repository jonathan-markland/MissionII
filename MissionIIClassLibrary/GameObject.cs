
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;
using GameClassLibrary.Walls;

namespace MissionIIClassLibrary
{
    public interface IGameBoard
    {
        void Add(GameObject o);
        void Remove(GameObject o);
        void ForEachObjectInPlayDo<A>(Action<A> theAction) where A : class;

        SpriteInstance ManSpriteInstance();
        void AddToPlayerInventory(Interactibles.InteractibleObject o);
        bool PlayerInventoryContains(Interactibles.InteractibleObject o);
        void PlayerIncrementScore(int deltaAmount);
        void PlayerGainLife();
        void PlayerLoseLife();
        // Missing drop from inventory
        // Missing multi-player

        void PrepareForNewLevel(int newLevelNumber);
        int GetLevelNumber();

        TileMatrix GetTileMatrix();
        // Can this know the tile dimensions?

        FoundDirections GetFreeDirections(Rectangle currentExtents);
        // Doable as extension except needs IsFloor predicate specific to game.  But users of
        // this are AI that could be re-usable.  TileMatrix could know IsFloor predicate?

        CollisionDetection.WallHitTestResult MoveManOnePixel(MovementDeltas movementDeltas);

        CollisionDetection.WallHitTestResult MoveAdversaryOnePixel(SpriteInstance spriteInstance, MovementDeltas movementDeltas);
        // THe base class for adversaries would need to be in the library.

        void StartBullet(SpriteInstance sourceSprite, MovementDeltas bulletDirection, bool increasesScore);
        void MoveRoomNumberByDelta(int roomNumberDelta);
        void PrepareForNewRoom();
        void ForEachThingWeHaveToFindOnThisLevel(Action<Interactibles.InteractibleObject> theAction);
        void Electrocute(ElectrocutionMethod electrocutionMethod);
        bool ManIsInvincible();
        void ManGainInvincibility();
    }



    public static class IGameBoardExtensionsForMissionII
    {
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
            var manCentre = gameBoard.ManSpriteInstance().Centre;
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

            gameBoard.ForEachObjectInPlayDo<MissionIIGameObject>(o =>
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

    }


    public struct ShotStruct
    {
        public bool Affirmed;
        public int ScoreIncrease;
    }


    public abstract class GameObject
    {
        public abstract void AdvanceOneCycle(IGameBoard theGameBoard, KeyStates theKeyStates);
        public abstract void Draw(IDrawingTarget drawingTarget);
        public abstract Rectangle GetBoundingRectangle();
        public abstract void ManWalkedIntoYou(IGameBoard theGameBoard);
        public abstract ShotStruct YouHaveBeenShot(IGameBoard theGameBoard, bool shotByMan);
        public abstract Point TopLeftPosition { get; set; }
        public abstract bool CanBeOverlapped { get; }
    }
}
