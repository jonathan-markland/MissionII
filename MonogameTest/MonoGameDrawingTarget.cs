using GameClassLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonogameTest
{
    public class MonoGameDrawingTarget: GameClassLibrary.IDrawingTarget
    {
        private SpriteBatch _spriteBatch;

        public MonoGameDrawingTarget(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
        }

        void IDrawingTarget.ClearScreen()
        {
            // TODO
        }

        void IDrawingTarget.DrawSprite(int x, int y, object hostImageObject)
        {
            var monoGameSprite = (Texture2D)hostImageObject;
            _spriteBatch.Draw(monoGameSprite, new Vector2(x, y), Color.White);
        }
    }
}