using GameClassLibrary.Input;

namespace MissionIIClassLibrary
{
    public abstract class BaseGameObject : GameObject
    {
        public virtual int KillScore { get { return 0; } }
    }
}
