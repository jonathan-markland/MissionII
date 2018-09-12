
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
        void ForEachObjectInPlayDo(Action<GameObject> theAction);

        void AddToPlayerInventory(Interactibles.InteractibleObject o);
        void IncrementScore(int deltaAmount);
        void GainLife();
        void LoseLife();
        int GetLevelNumber();

        TileMatrix GetTileMatrix();
        FoundDirections GetFreeDirections(Rectangle currentExtents);
        CollisionDetection.WallHitTestResult MoveManOnePixel(MovementDeltas movementDeltas);
        CollisionDetection.WallHitTestResult MoveAdversaryOnePixel(SpriteInstance spriteInstance, MovementDeltas movementDeltas);

        void StartBullet(SpriteInstance sourceSprite, MovementDeltas bulletDirection, bool increasesScore);
        void MoveRoomNumberByDelta(int roomNumberDelta);
        void PrepareForNewRoom();
        void PrepareForNewLevel(int newLevelNumber);
        uint KillThingsIfShotAndGetHitCount(GameObjects.Bullet theBullet);
        bool DroidsExistInRoom();
        void ForEachThingWeHaveToFindOnThisLevel(Action<Interactibles.InteractibleObject> theAction);
        GameClassLibrary.Math.Point GetCornerFurthestAwayFromMan();
        bool PlayerInventoryContains(Interactibles.InteractibleObject o);
        SpriteInstance ManSpriteInstance();
        void Electrocute(ElectrocutionMethod electrocutionMethod);
        bool ManIsInvincible();
        void ManGainInvincibility();
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
