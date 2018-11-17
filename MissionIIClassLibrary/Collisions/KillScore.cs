
namespace MissionIIClassLibrary.Collisions
{
    public static class KillScore
    {
        public static int Get(object o)
        {
            // Lean to functional programming!

            if (o is Droids.BouncingDroid) return Constants.DestroyerDroidKillScore;
            if (o is Droids.DestroyerDroid) return Constants.DestroyerDroidKillScore;
            if (o is Droids.GuardianDroid) return Constants.GuardianDroidKillScore;
            if (o is Droids.HomingDroid) return Constants.HomingDroidKillScore;
            if (o is Droids.KamikazeDroid) return Constants.DestroyerDroidKillScore;
            if (o is Droids.LinearMoverDroid) return Constants.DestroyerDroidKillScore;
            if (o is Droids.WanderingDroid) return Constants.WanderingDroidKillScore;
            if (o is Droids.WanderingMineDroid) return Constants.WanderingMineDroidKillScore;
            return 0;
        }
    }
}
