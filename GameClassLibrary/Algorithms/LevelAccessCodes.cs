
namespace GameClassLibrary.Algorithms
{
    public class LevelAccessCodes
    {
        public static string GetForLevel(int levelNumber)
        {
            System.Diagnostics.Debug.Assert(levelNumber >= 1 && levelNumber <= 8);

            int lastAccessCodeInt = 0;
            int v = levelNumber;
            string s = string.Empty;

            for (int i = 0; i < 4; i++)
            {
                var thisAccessCodeInt = AccessCodeInt(lastAccessCodeInt, v);
                s += AccessCodeIntToChar(thisAccessCodeInt);
                lastAccessCodeInt = thisAccessCodeInt;
                v >>= 2;
            }
            return s;
        }

        private static int AccessCodeInt(int lastAccessCodeInt, int arbitraryValue)
        {
            arbitraryValue &= 3;
            var n = (lastAccessCodeInt + 1 + arbitraryValue) % 5;
            System.Diagnostics.Debug.Assert(n != lastAccessCodeInt);
            return n;
        }

        private static string AccessCodeIntToChar(int accessCodeInt)
        {
            return ("UDLRF"[accessCodeInt]).ToString();
        }
    }
}
