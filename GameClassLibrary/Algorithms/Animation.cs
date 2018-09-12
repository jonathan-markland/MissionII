
namespace GameClassLibrary.Algorithms
{
    public static class Animation
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
