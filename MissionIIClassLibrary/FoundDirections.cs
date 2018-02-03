using System;

namespace MissionIIClassLibrary
{
    public struct FoundDirections
    {
        public int Count;
        public int DirectionsMask; // Follows MovementDeltas convention.

        public int Choose(int theIndex)
        {
            System.Diagnostics.Debug.Assert(theIndex < Count);
            int bitMask = 1;
            for (int i=0; i<8; i++)
            {
                if ((DirectionsMask & bitMask) != 0)
                {
                    if (theIndex == 0) return i;
                    --theIndex;
                }
                bitMask <<= 1;
            }
            throw new Exception("Unexpected failure to find direction.");
        }
    }
}
