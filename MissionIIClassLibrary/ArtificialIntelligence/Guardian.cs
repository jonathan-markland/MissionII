﻿
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.Graphics;

namespace MissionIIClassLibrary.ArtificialIntelligence
{
    public class Guardian: AbstractIntelligenceProvider
    {
        private int _cycleCounter;
        private int _facingDirection = 0;
        private MovementDeltas _movementDeltas = new MovementDeltas(0, 0);



        public override void AdvanceOneCycle(IGameBoard theGameBoard, GameObject gameObject)
        {
            ++_cycleCounter;

            if (_movementDeltas.Stationary)
            {
                _facingDirection = GameClassLibrary.Math.Rng.Generator.Next(8);
                _movementDeltas = MovementDeltas.ConvertFromFacingDirection(_facingDirection);
            }
            else
            {
                var hitResult = theGameBoard.MoveAdversaryOnePixel(gameObject, _movementDeltas);  // TODO: differentiate walls/other droids
                if (hitResult != CollisionDetection.WallHitTestResult.NothingHit)
                {
                    if (gameObject.GetBoundingRectangle().Intersects(
                        theGameBoard.GetMan().GetBoundingRectangle().Inflate(5)))
                    {
                        theGameBoard.Electrocute(ElectrocutionMethod.ByDroid);
                    }
                    else
                    {
                        _facingDirection = (_facingDirection + 4) & 7;  // TODO: reverse direction function
                        _movementDeltas = MovementDeltas.ConvertFromFacingDirection(_facingDirection);
                    }
                }
            }
        }
    }
}
