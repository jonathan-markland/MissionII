using GameClassLibrary.Input;

namespace MissionIIClassLibrary
{
    public abstract class MissionIIGameObject : GameObject
    {
        public virtual int KillScore { get { return 0; } }
    }
}
