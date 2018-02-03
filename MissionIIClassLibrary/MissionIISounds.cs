using System;

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
        public static SoundTraits StunGhost;
        public static SoundTraits FootStep1;
        public static SoundTraits FootStep2;

        private static Action<SoundTraits> HostPlaySoundAction;

        public static void Init(Action<SoundTraits> hostPlaySoundAction)
        {
            HostPlaySoundAction = hostPlaySoundAction;
        }

        public static void Play(SoundTraits theSound)
        {
            HostPlaySoundAction(theSound);
        }

        public static void Load(Func<string, object> hostSoundSupplier)
        {
            Func<string, SoundTraits> loadSound = (soundName) =>
            {
                return new SoundTraits { HostObject = hostSoundSupplier(soundName) };
            };

            DroidFiring = loadSound("DroidFiringSound");
            Electrocution = loadSound("ElectrocutionSound");
            EnteringLevel = loadSound("EnteringLevelSound");
            ExtraLife = loadSound("ExtraLifeSound");
            GameOver = loadSound("GameOverSound");
            GhostAppearing = loadSound("GhostAppearingSound");
            ManFiring = loadSound("ManFiringSound");
            ManGrunt = loadSound("ManGruntSound");
            PauseMode = loadSound("PauseModeSound");
            PickUpObject = loadSound("PickUpObjectSound");
            SafeActivated = loadSound("SafeActivatedSound");
            Intro = loadSound("IntroSound");
            Explosion = loadSound("ExplosionSound");
            Bonus = loadSound("BonusSound");
            StunGhost = loadSound("StunGhostSound");
            FootStep1 = loadSound("Footstep1Sound");
            FootStep2 = loadSound("Footstep2Sound");
        }
    }
}
