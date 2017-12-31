namespace GameClassLibrary
{
    public class GameMode<K>
    {
        public virtual void AdvanceOneCycle(K theKeyStates)
        {
            // No base action
        }

        public virtual void Draw(IDrawingTarget drawingTarget)
        {
            // No base action
        }
    }
}
