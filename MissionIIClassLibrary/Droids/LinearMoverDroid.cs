
using System;
using GameClassLibrary.Math;
using GameClassLibrary.ArtificialIntelligence;
using GameClassLibrary.Walls;
using GameClassLibrary.GameBoard;
using GameClassLibrary.Sound;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.Droids
{
    public class LinearMoverDroid : BaseDroid  // TODO: This is a development test and should be removed.
    {
        public LinearMoverDroid(Action<GameObject> manWalksIntoDroidAction, Func<Rectangle> getManExtents, Action<GameObject, SpriteTraits, SoundTraits> startExplosion)
            : base(
                  MissionIISprites.Monster3,
                  MissionIISprites.Explosion,
                  MissionIISounds.Explosion,
                  // TODO: Demo no longer works since we are using model-space coordinates:
                  manWalksIntoDroidAction,
                  startExplosion)
        {
            base.SetIntelligenceProvider(
                LinearMover.NewLinearMover(
                    this, new Point(100, 50), new Point(200, 50), 1, 
                    manWalksIntoDroidAction, getManExtents));
        }

        public override int KillScore
        {
            get { return Constants.DestroyerDroidKillScore; }
        }
    }
}
