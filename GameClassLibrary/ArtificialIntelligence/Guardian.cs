
using System;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.GameBoard;

namespace GameClassLibrary.ArtificialIntelligence
{
    public class Guardian: AbstractIntelligenceProvider
    {
		private readonly Action _manDestroyAction;
        private readonly Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> _moveAdversaryOnePixel;
        private readonly Func<Rectangle> _getManExtents;
        private readonly GameObject _gameObject;

        private int _facingDirection = 0;
        private MovementDeltas _movementDeltas = MovementDeltas.Stationary;



        public Guardian(
            GameObject gameObject,
            Action manDestroyAction,
            Func<GameObject, MovementDeltas, CollisionDetection.WallHitTestResult> moveAdversaryOnePixel,
            Func<Rectangle> getManExtents)
        {
            _manDestroyAction = manDestroyAction;
            _moveAdversaryOnePixel = moveAdversaryOnePixel;
            _getManExtents = getManExtents;
            _gameObject = gameObject;
        }



        public override void AdvanceOneCycle()
        {
            if (_movementDeltas.IsStationary)
            {
                _facingDirection = GameClassLibrary.Math.Rng.Generator.Next(8);
                _movementDeltas = MovementDeltas.ConvertFromFacingDirection(_facingDirection);
            }
            else
            {
                var hitResult = _moveAdversaryOnePixel(_gameObject, _movementDeltas);  // TODO: differentiate walls/other droids
                if (hitResult != CollisionDetection.WallHitTestResult.NothingHit)
                {
                    if (_gameObject.GetBoundingRectangle().Intersects(
                        _getManExtents().Inflate(5)))
                    {
                        _manDestroyAction();
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
