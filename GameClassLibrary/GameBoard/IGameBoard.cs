
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;

namespace GameClassLibrary.GameBoard
{
    public interface IGameBoard
    {
        void PrepareForNewLevel(int newLevelNumber);
        int GetLevelNumber();

        ArraySlice2D<Tile> GetLevelTileMatrix();
        int GetTileWidth();
        int GetTileHeight();

        void Add(GameObject o);
        void Remove(GameObject o);
        void ForEachObjectInPlayDo<A>(Action<A> theAction) where A : class;

        GameObject GetMan();  // TODO: Get rid of this function.  Only need extents rectangle.
        
        // TODO: inconsistent terms Man vs. Player in this section:
        void AddToPlayerInventory(InteractibleObject o);
        bool PlayerInventoryContains(InteractibleObject o);
        void PlayerIncrementScore(int deltaAmount);
        void PlayerGainLife();
        void PlayerLoseLife();
        bool ManIsInvincible();
        void ManGainInvincibility();
        // Missing drop from inventory
        // Missing multi-player

        CollisionDetection.WallHitTestResult MoveManOnePixel(MovementDeltas movementDeltas);

        CollisionDetection.WallHitTestResult MoveAdversaryOnePixel(GameObject adversaryObject, MovementDeltas movementDeltas);
        // THe base class for adversaries would need to be in the library.
    }
}
