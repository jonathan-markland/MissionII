using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameClassLibrary
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


        public static int GetDirectionIndex(CybertronKeyStates keyStates)
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


        private static MovementDeltas[] g_MovementDeltas = new MovementDeltas[]
        {
            new MovementDeltas { dx = 0, dy = -1 },
            new MovementDeltas { dx = 1, dy = -1 },
            new MovementDeltas { dx = 1, dy = 0 },
            new MovementDeltas { dx = 1, dy = 1 },
            new MovementDeltas { dx = 0, dy = 1 },
            new MovementDeltas { dx = -1, dy = 1 },
            new MovementDeltas { dx = -1, dy = 0 },
            new MovementDeltas { dx = -1, dy = -1 },
        };


        public static MovementDeltas GetMovementDeltas(int facingDirection)
        {
            return g_MovementDeltas[facingDirection];
        }


        public static MovementDeltas GetMovementDeltas(CybertronKeyStates keyStates)
        {
            return new MovementDeltas(
                (keyStates.Left ? -1 : 0) + (keyStates.Right ? 1 : 0),
                (keyStates.Up ? -1 : 0) + (keyStates.Down ? 1 : 0)
            );
        }


        public static void Shuffle<T>(this IList<T> list, Random rnd)
        {
            for (var i = 0; i < list.Count; i++)
                list.Swap(i, rnd.Next(i, list.Count));
        }

        public static void Swap<T>(this IList<T> list, int i, int j)
        {
            var temp = list[i];
            list[i] = list[j];
            list[j] = temp;
        }

    }
}
