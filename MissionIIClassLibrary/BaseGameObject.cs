namespace MissionIIClassLibrary
{
    public abstract class BaseGameObject : GameObject<MissionIIGameBoard, MissionIIKeyStates>
    {
        public virtual int KillScore { get { return 0; } }
    }
}
