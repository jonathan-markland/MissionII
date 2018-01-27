using System;

namespace MissionIIClassLibrary
{
    public class SoundTraits
    {
        public object HostObject;
    }

    public static class CybertronSounds
    {
        public static SoundTraits DroidFiring; //
        public static SoundTraits Electrocution; //
        public static SoundTraits EnteirngLevel; //
        public static SoundTraits ExtraLife; //
        public static SoundTraits GameOverSound; //
        public static SoundTraits GhostAppearing; //
        public static SoundTraits ManFiring; //
        public static SoundTraits ManGrunt; //
        public static SoundTraits PauseMode; //
        public static SoundTraits PickUpObject; // 
        public static SoundTraits SafeActivated; //
        public static SoundTraits IntroSound; //
        public static SoundTraits ExplosionSound; //
        public static SoundTraits BonusSound; //
        public static SoundTraits StunGhostSound;
        public static SoundTraits Footstep1Sound;
        public static SoundTraits Footstep2Sound;

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

            DroidFiring = loadSound("DroidFiring");
            Electrocution = loadSound("Electrocution");
            EnteirngLevel = loadSound("EnteirngLevel");
            ExtraLife = loadSound("ExtraLife");
            GameOverSound = loadSound("GameOverSound");
            GhostAppearing = loadSound("GhostAppearing");
            ManFiring = loadSound("ManFiring");
            ManGrunt = loadSound("ManGrunt");
            PauseMode = loadSound("PauseMode");
            PickUpObject = loadSound("PickUpObject");
            SafeActivated = loadSound("SafeActivated");
            IntroSound = loadSound("IntroSound");
            ExplosionSound = loadSound("ExplosionSound");
            BonusSound = loadSound("BonusSound");
            StunGhostSound = loadSound("StunGhostSound");
            Footstep1Sound = loadSound("Footstep1Sound");
            Footstep2Sound = loadSound("Footstep2Sound");
        }
    }
}
