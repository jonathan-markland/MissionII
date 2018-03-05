
using System;
using System.Collections.Generic;

namespace GameClassLibrary.Sound
{
    public class SoundTraits
    {
        private static Func<string, HostSuppliedSound> _hostSoundSupplier;
        private static Action<HostSuppliedSound> _hostPlaySoundAction;
        private static Random _rndGen;
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
    }
}
