using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.GameBoard;

namespace MissionIIClassLibrary.Interactibles
{
    public class Potion : MissionIIInteractibleObject
    {
        public Potion(int roomNumber, Action<InteractibleObject, int> collectObject)
            : base(new SpriteInstance { Traits = MissionIISprites.Potion }, roomNumber, collectObject)
        {
        }

        public override void ManWalkedIntoYou(IGameBoard theGameBoard)
        {
            theGameBoard.PlayerGainLife();
            theGameBoard.Remove(this);
            base.MarkCollected();
        }

        public override int CollectionScore => 0;
    }
}
