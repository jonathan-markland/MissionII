using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Interactibles
{
    public class InvincibilityAmulet : MissionIIInteractibleObject
    {
        public InvincibilityAmulet(int roomNumber)
            : base(new SpriteInstance { Traits = MissionIISprites.InvincibilityAmulet }, roomNumber)
        {
        }

        public override void ManWalkedIntoYou(IGameBoard theGameBoard)
        {
            theGameBoard.ManGainInvincibility();
            theGameBoard.Remove(this);
            MissionIISounds.InvincibilityAmuletSound.Play();
        }

        public override int CollectionScore => Constants.InvincibilityAmuletScore;
    }
}
