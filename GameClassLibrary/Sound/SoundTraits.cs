
using System;
using System.Collections.Generic;

namespace GameClassLibrary.Sound
{
    public class SoundTraits
    {
        private static Func<string, HostSuppliedSound> _hostSoundSupplier;
        private static Action<HostSuppliedSound> _hostPlaySoundAction;
        private static Action<HostSuppliedSound> _hostPlayMusicAction;
        private static Action _hostStopMusicAction;
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

        public static void InitMusicPlay(
            Action<HostSuppliedSound> hostPlayMusicAction)
        {
            _hostPlayMusicAction = hostPlayMusicAction;
        }

        public static void InitMusicStop(
            Action hostStopMusicAction)
        {
            _hostStopMusicAction = hostStopMusicAction;
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

        private void ChooseHostSoundAndDo(Action<HostSuppliedSound> theAction)
        {
            if (_hostSoundObjects.Count == 1)
            {
                theAction(_hostSoundObjects[0]);
            }
            else if (_hostSoundObjects.Count > 1)
            {
                theAction(_hostSoundObjects[_rndGen.Next(_hostSoundObjects.Count)]);
            }
        }

        /// <summary>
        /// Play a new instance of the sound.
        /// </summary>
        public void Play()
        {
            ChooseHostSoundAndDo(_hostPlaySoundAction);
        }

        /// <summary>
        /// Play an instance of this sound as background music.
        /// This is IGNORED if music is currently playing -- you should stop it first.
        /// This can be stopped with the static StopBackgroundMusic() command.
        /// </summary>
        public void PlayAsBackgroundMusic()
        {
            ChooseHostSoundAndDo(_hostPlayMusicAction);
        }

        public static void StopBackgroundMusic()
        {
            _hostStopMusicAction();
        }
    }
}
