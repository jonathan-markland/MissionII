
namespace MissionIIClassLibrary.Collisions
{
    public class DroidOverlap
    {
        /// <summary>
        /// Returns true if droid can overlap 'o'
        /// </summary>
        public static bool IsAllowed(object o)
        {
            return !(o is Droids.BaseDroid) && !(o is GameObjects.Man);
        }
    }
}
