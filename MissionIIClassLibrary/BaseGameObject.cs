namespace MissionIIClassLibrary
{
    public abstract class BaseGameObject : GameObject<CybertronGameBoard, CybertronKeyStates>
    {
        public virtual int KillScore { get { return 0; } }
    }
}
