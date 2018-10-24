using System;
using GameClassLibrary.Graphics;
using GameClassLibrary.GameBoard;

namespace MissionIIClassLibrary.Interactibles
{
    public class Potion : MissionIIInteractibleObject
    {
        private readonly Action<GameObject> _gainLife;

        public Potion(int roomNumber, Action<InteractibleObject, int> collectObject, Action<GameObject> gainLife)
            : base(new SpriteInstance { Traits = MissionIISprites.Potion }, roomNumber, collectObject)
        {
            _gainLife = gainLife;
        }

        public override void ManWalkedIntoYou()
        {
            _gainLife(this);
            base.MarkCollected();
        }

        public override int CollectionScore => 0;
    }
}
