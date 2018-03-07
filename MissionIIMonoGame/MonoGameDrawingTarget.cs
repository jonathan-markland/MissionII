using MissionIIClassLibrary;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameClassLibrary.Graphics;

namespace MissionIIMonoGame
{
    public class MonoGameDrawingTarget: IDrawingTarget
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

        void IDrawingTarget.DrawSprite(int x, int y, HostSuppliedSprite hostSuppliedSprite)
        {
            var monoGameSprite = (Texture2D) hostSuppliedSprite.HostObject;
            _spriteBatch.Draw(monoGameSprite, new Vector2(x, y), Color.White);
        }

        void IDrawingTarget.DrawSpritePieceStretched(
            int sx, int sy, int sw, int sh, 
            int dx, int dy, int dw, int dh, 
            HostSuppliedSprite hostSuppliedSprite)
        {
            var monoGameSprite = (Texture2D)hostSuppliedSprite.HostObject;
            _spriteBatch.Draw(
                monoGameSprite,
                new Rectangle(dx, dy, dw, dh),
                new Rectangle(sx, sy, sw, sh),
                Color.White);
        }
    }
}