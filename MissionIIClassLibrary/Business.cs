using System;
using System.Collections.Generic;
using GameClassLibrary.Math;

namespace MissionIIClassLibrary
{
    public static class Business
    {
        public static byte ToGreyscale(uint rgbValue)
        {
            return (byte)((rgbValue >> 8) & 255); // TODO: fix
        }



        public static void Animate(ref int animationCountdown, ref int imageIndex, int animationCountdownReset, int maxImageCount)
        {
            if (animationCountdown <= 0)
            {
                animationCountdown = animationCountdownReset;
                ++imageIndex;
            }
            if (imageIndex >= maxImageCount)
            {
                imageIndex = 0;
            }
            --animationCountdown;
        }


        public static int GetDirectionIndex(MissionIIKeyStates keyStates)
        {
            // Clockwise numbering
            // TODO:  Implementation biases certain directions when more than 2 keys held.
            if (keyStates.Up)
            {
                if (keyStates.Left) return 7;
                if (keyStates.Right) return 1;
                return 0;
            }
            if (keyStates.Down)
            {
                if (keyStates.Left) return 5;
                if (keyStates.Right) return 3;
                return 4;
            }
            if (keyStates.Left) return 6;
            if (keyStates.Right) return 2;
            return -1; // No keys held.  Cannot determine a direction.
        }



        public static int GetDirectionFacingAwayFromWalls(WallMatrix fileWallData, Point startCluster)
        {
            var clusterCanvas = new ClusterCanvas(
                fileWallData, startCluster.X, startCluster.Y, Constants.SourceClusterSide);

            if (clusterCanvas.IsSpace(2)) return 0;
            if (clusterCanvas.IsSpace(4)) return 6;
            if (clusterCanvas.IsSpace(6)) return 2;
            if (clusterCanvas.IsSpace(8)) return 4;
            throw new Exception("Cannot establish an exit direction, all sides of cluster have walls.");
        }



        private static MovementDeltas[] g_MovementDeltas = new MovementDeltas[]
        {
            new MovementDeltas { dx =  0, dy = -1 }, // up
            new MovementDeltas { dx =  1, dy = -1 }, // up right
            new MovementDeltas { dx =  1, dy =  0 }, // right
            new MovementDeltas { dx =  1, dy =  1 }, // down right
            new MovementDeltas { dx =  0, dy =  1 }, // down
            new MovementDeltas { dx = -1, dy =  1 }, // down left
            new MovementDeltas { dx = -1, dy =  0 }, // left 
            new MovementDeltas { dx = -1, dy = -1 }, // left up
        };


        public static MovementDeltas GetMovementDeltas(int facingDirection)
        {
            return g_MovementDeltas[facingDirection];
        }


        public static MovementDeltas GetMovementDeltas(MissionIIKeyStates keyStates)
        {
            return new MovementDeltas(
                (keyStates.Left ? -1 : 0) + (keyStates.Right ? 1 : 0),
                (keyStates.Up ? -1 : 0) + (keyStates.Down ? 1 : 0)
            );
        }


        public static MovementDeltas GetMovementDeltasToHeadTowards(SpriteInstance aggressorSprite, SpriteInstance targetSprite)
        {
            var targetCentre = targetSprite.Centre;
            var aggressorCentre = aggressorSprite.Centre;

            int dx = 0;
            if (targetCentre.X < aggressorCentre.X) dx = -1;
            if (targetCentre.X > aggressorCentre.X) dx = 1;

            int dy = 0;
            if (targetCentre.Y < aggressorCentre.Y) dy = -1;
            if (targetCentre.Y > aggressorCentre.Y) dy = 1;

            return new MovementDeltas(dx, dy);
        }




        public static Func<object, uint[]> GetSpriteDataAsUintArray;
    }
}
