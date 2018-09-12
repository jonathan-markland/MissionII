
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
        FoundDirections GetFreeDirections(Rectangle currentExtents);
        CollisionDetection.WallHitTestResult MoveManOnePixel(MovementDeltas movementDeltas);
        CollisionDetection.WallHitTestResult MoveAdversaryOnePixel(SpriteInstance spriteInstance, MovementDeltas movementDeltas);

        void StartBullet(SpriteInstance sourceSprite, MovementDeltas bulletDirection, bool increasesScore);
        void MoveRoomNumberByDelta(int roomNumberDelta);
        void PrepareForNewRoom();
        uint KillThingsIfShotAndGetHitCount(GameObjects.Bullet theBullet);
        void ForEachThingWeHaveToFindOnThisLevel(Action<Interactibles.InteractibleObject> theAction);
        GameClassLibrary.Math.Point GetCornerFurthestAwayFromMan();
        SpriteInstance ManSpriteInstance();
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

    }

    public abstract class GameObject
    {
        public abstract void AdvanceOneCycle(IGameBoard theGameBoard, KeyStates theKeyStates);
        public abstract void Draw(IDrawingTarget drawingTarget);
        public abstract Rectangle GetBoundingRectangle();
        public abstract void ManWalkedIntoYou(IGameBoard theGameBoard);
        public abstract bool YouHaveBeenShot(IGameBoard theGameBoard, bool shotByMan);
        public abstract Point TopLeftPosition { get; set; }
        public abstract bool CanBeOverlapped { get; }
    }
}
