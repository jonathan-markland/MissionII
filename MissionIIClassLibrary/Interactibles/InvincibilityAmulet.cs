
using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.GameBoard;

namespace MissionIIClassLibrary.Interactibles
{
    public class InvincibilityAmulet : MissionIIInteractibleObject
    {
        public InvincibilityAmulet(int roomNumber, Action<InteractibleObject, int> collectObject)
            : base(new SpriteInstance { Traits = MissionIISprites.InvincibilityAmulet }, roomNumber, collectObject)
        {
        }

        public override void ManWalkedIntoYou(IGameBoard theGameBoard)
        {
            theGameBoard.ManGainInvincibility();
            theGameBoard.Remove(this);
            MissionIISounds.InvincibilityAmuletSound.Play();
            MarkCollected();
        }

        public override int CollectionScore => Constants.InvincibilityAmuletScore;
    }
}
