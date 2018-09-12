
using System;
using GameClassLibrary.Math;

namespace MissionIIClassLibrary.Droids
{
    public class WanderingDroid : BaseDroid
    {
        public WanderingDroid(Func<Rectangle, FoundDirections> freeDirectionFinder)
            : base(MissionIISprites.Monster2, new ArtificialIntelligence.SingleMinded(freeDirectionFinder))
        {
        }

        public override int KillScore
        {
            get { return Constants.WanderingDroidKillScore; }
        }
    }
}
