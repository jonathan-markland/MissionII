using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MissionIIClassLibrary.Interactibles
{
    public class InvincibilityAmulet : InteractibleObject
    {
        public InvincibilityAmulet(int roomNumber)
            : base(new SpriteInstance { Traits = MissionIISpriteTraits.InvincibilityAmulet }, roomNumber)
        {
        }

        public override void ManWalkedIntoYou(MissionIIGameBoard theGameBoard)
        {
            theGameBoard.Man.GainInvincibility();
            RemoveThisObject(theGameBoard);
            MissionIISounds.Play(MissionIISounds.InvincibilityAmuletSound);
        }

        public override int CollectionScore => MissionIIGameBoardConstants.InvincibilityAmuletScore;
    }
}
