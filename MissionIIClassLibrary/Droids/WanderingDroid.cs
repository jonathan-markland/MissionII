﻿
namespace MissionIIClassLibrary.Droids
{
    public class WanderingDroid : BaseDroid
    {
        public WanderingDroid()
            : base(MissionIISprites.Monster2, new ArtificialIntelligence.SingleMinded())
        {
        }

        public override int KillScore
        {
            get { return Constants.WanderingDroidKillScore; }
        }
    }
}
