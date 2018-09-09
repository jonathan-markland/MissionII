using System;
using System.Collections.Generic;
using GameClassLibrary.Math;
using GameClassLibrary.Walls;
using GameClassLibrary.Graphics;
using GameClassLibrary.Input;

namespace MissionIIClassLibrary
{
    public static class Business
    {
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


        public static int GetDirectionIndex(KeyStates keyStates)
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



        public static MovementDeltas GetMovementDeltas(KeyStates keyStates)
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
    }
}
