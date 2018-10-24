
using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.GameBoard;

namespace MissionIIClassLibrary.Interactibles
{
    public class InvincibilityAmulet : MissionIIInteractibleObject
    {
        private readonly Action<GameObject> _gainInvincibility;

        // TODO: This does not need collectObject because we override the base!
        public InvincibilityAmulet(int roomNumber, Action<InteractibleObject, int> collectObject, Action<GameObject> gainInvincibility)
            : base(new SpriteInstance { Traits = MissionIISprites.InvincibilityAmulet }, roomNumber, collectObject)
        {
            _gainInvincibility = gainInvincibility;
        }

        public override void ManWalkedIntoYou(IGameBoard theGameBoard)
        {
            _gainInvincibility(this);
            MarkCollected();
        }

        public override int CollectionScore => Constants.InvincibilityAmuletScore;
    }
}
