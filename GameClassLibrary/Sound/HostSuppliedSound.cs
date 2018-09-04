namespace GameClassLibrary.Sound
{
    /// <summary>
    /// A wrapper for a host-supplied sound object, where the client
    /// does not need to know the exact type.
    /// </summary>
    public struct HostSuppliedSound
    {
        public HostSuppliedSound(object hostSoundObject)
        {
            HostSoundObject = hostSoundObject;
        }

        public object HostSoundObject { get; private set; }
    }
}
