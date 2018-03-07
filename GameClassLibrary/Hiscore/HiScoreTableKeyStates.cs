
namespace GameClassLibrary.Hiscore
{
    /// <summary>
    /// The live key states used to operate the "enter your name" feature.
    /// Just pass in whether the keys are UP (false) or DOWN (true).
    /// </summary>
    public struct HiScoreTableKeyStates
    {
        public bool Up;
        public bool Down;
        public bool Left;
        public bool Fire;

        public bool AllReleased
        {
            get
            {
                return !Up && !Down && !Left && !Fire;
            }
        }
    }
}
