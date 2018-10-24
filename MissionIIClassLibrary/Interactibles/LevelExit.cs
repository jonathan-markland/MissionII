using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.GameBoard;

namespace MissionIIClassLibrary.Interactibles
{
    public class LevelExit : MissionIIInteractibleObject
    {
        // TODO: This should not need to be passed the collectObject function!
        public LevelExit(int roomNumber, Action<InteractibleObject, int> collectObject) 
            : base(new SpriteInstance { Traits = MissionIISprites.LevelExit }, roomNumber, collectObject)
        {
        }

        public override int CollectionScore => 0;

        public override void ManWalkedIntoYou(IGameBoard theGameBoard)
        {
            bool carryingEverything = true;
            ((MissionIIGameBoard) theGameBoard).ForEachThingWeHaveToFindOnThisLevel(o => 
            {
                if (!theGameBoard.PlayerInventoryContains(o))
                {
                    carryingEverything = false;
                }
            });
            if (carryingEverything)
            {
                GameClassLibrary.Modes.GameMode.ActiveMode = new Modes.LeavingLevel(theGameBoard);
            }
        }
    }
}
