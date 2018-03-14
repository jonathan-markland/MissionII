
using GameClassLibrary.Sound;

namespace MissionIIClassLibrary
{
    public static class MissionIISounds
    {
        public static SoundTraits DroidFiring; 
        public static SoundTraits Electrocution; 
        public static SoundTraits EnteringLevel; 
        public static SoundTraits ExtraLife; 
        public static SoundTraits GameOver; 
        public static SoundTraits GhostAppearing; 
        public static SoundTraits ManFiring; 
        public static SoundTraits ManGrunt; 
        public static SoundTraits PauseMode; 
        public static SoundTraits PickUpObject;
        public static SoundTraits SafeActivated; 
        public static SoundTraits Intro; 
        public static SoundTraits Explosion; 
        public static SoundTraits Bonus;
        public static SoundTraits DuoBonus;
        public static SoundTraits StunGhost;
        public static SoundTraits FootStep1;
        public static SoundTraits FootStep2;
        public static SoundTraits InvincibilityAmuletSound;

        public static void Load()
        {
            Bonus = new SoundTraits("BonusSound", 1);
            DroidFiring = new SoundTraits("DroidFiringSound", 1);
            DuoBonus = new SoundTraits("DuoBonusSound", 1);
            Electrocution = new SoundTraits("ElectrocutionSound", 1);
            EnteringLevel = new SoundTraits("EnteringLevelSound", 1);
            Explosion = new SoundTraits("ExplosionSound", 1);
            ExtraLife = new SoundTraits("ExtraLifeSound", 1);
            FootStep1 = new SoundTraits("Footstep1Sound", 1);
            FootStep2 = new SoundTraits("Footstep2Sound", 1);
            GameOver = new SoundTraits("GameOverSound", 4);
            GhostAppearing = new SoundTraits("GhostAppearingSound", 1);
            Intro = new SoundTraits("IntroSound", 1);
            InvincibilityAmuletSound = new SoundTraits("InvincibilityAmuletSound", 1);
            ManFiring = new SoundTraits("ManFiringSound", 1);
            ManGrunt = new SoundTraits("ManGruntSound", 2);
            PauseMode = new SoundTraits("PauseModeSound", 1);
            PickUpObject = new SoundTraits("PickUpObjectSound", 1);
            SafeActivated = new SoundTraits("SafeActivatedSound", 1);
            StunGhost = new SoundTraits("StunGhostSound", 1);
        }
    }
}
