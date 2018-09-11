using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Interactibles
{
    public class InvincibilityAmulet : InteractibleObject
    {
        public InvincibilityAmulet(int roomNumber)
            : base(new SpriteInstance { Traits = MissionIISprites.InvincibilityAmulet }, roomNumber)
        {
        }

        public override void ManWalkedIntoYou(IGameBoard theGameBoard)
        {
            theGameBoard.Man.GainInvincibility();
            RemoveThisObject(theGameBoard);
            MissionIISounds.InvincibilityAmuletSound.Play();
        }

        public override int CollectionScore => Constants.InvincibilityAmuletScore;
    }
}
