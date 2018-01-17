namespace GameClassLibrary
{
    public abstract class CybertronGameObject : GameObject<CybertronGameBoard, CybertronKeyStates>
    {
        public virtual int KillScore { get { return 0; } }
    }
}
