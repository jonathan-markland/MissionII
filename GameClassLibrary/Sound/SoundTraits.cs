
using System;
using System.Collections.Generic;

namespace GameClassLibrary.Sound
{
    public class SoundTraits
    {
        private static Func<string, HostSuppliedSound> _hostSoundSupplier;
        private static Action<HostSuppliedSound> _hostPlaySoundAction;
        private static Random _rndGen;
        // private static Action<HostSuppliedSound> _hostCreateAndPlaySharedSoundAction;
        // private static Action<HostSuppliedSound> _hostStopSharedSoundPlayingAction;
        private List<HostSuppliedSound> _hostSoundObjects;

        public static void InitSoundSupplier(
            Func<string, HostSuppliedSound> hostSoundSupplier)
        {
            _hostSoundSupplier = hostSoundSupplier;
            _rndGen = new Random(0);
        }

        public static void InitSoundPlay(
            Action<HostSuppliedSound> hostPlaySoundAction)
        {
            _hostPlaySoundAction = hostPlaySoundAction;
            _rndGen = new Random(0);
        }

        public SoundTraits(string soundName, int soundCount)
        {
            var hostSoundObjects = new List<HostSuppliedSound>();

            for (int i = 1; i <= soundCount; i++)
            {
                var thisHostSoundInfo = _hostSoundSupplier((soundCount == 1) ? soundName : soundName + "_" + i);
                hostSoundObjects.Add(thisHostSoundInfo);
            }

            _hostSoundObjects = hostSoundObjects;
        }

        /// <summary>
        /// Play a new instance of the sound.
        /// </summary>
        public void Play()
        {
            if (_hostSoundObjects.Count == 1)
            { 
                _hostPlaySoundAction(_hostSoundObjects[0]);
            }
            else if (_hostSoundObjects.Count > 1)
            {
                _hostPlaySoundAction(_hostSoundObjects[_rndGen.Next(_hostSoundObjects.Count)]);
            }
        }

        /// <summary>
        /// Creates a shared instance of the sound, and plays it.
        /// Unless the shared instance already exists, in which case we don't create again.
        /// If the shared instances is already playing, this call has no effect.
        /// </summary>
        public void EnsurePlaying()
        {
            
        }

        /// <summary>
        /// If a shared instance of this sound exists, then stop it playing.
        /// If no shared instance exists, or the shared instance isn't playing,
        /// then this takes no effect.
        /// </summary>
        public void EnsureStopped()
        {

        }
    }
}
