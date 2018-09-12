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

    }
}
