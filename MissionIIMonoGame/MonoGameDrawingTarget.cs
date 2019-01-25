using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GameClassLibrary.Graphics;

namespace MissionII
{
    public class MonoGameDrawingTarget: IDrawingTarget
    {
        private SpriteBatch _spriteBatch;
        private int _originX;
        private int _originY;

        public MonoGameDrawingTarget(SpriteBatch spriteBatch)
        {
            _spriteBatch = spriteBatch;
            _originX = 0;
            _originY = 0;
        }

        void IDrawingTarget.ClearScreen()
        {
            // TODO
        }

        void IDrawingTarget.DeltaOrigin(int dx, int dy)
        {
            _originX += dx;
            _originY += dy;
        }

        void IDrawingTarget.DrawSprite(int x, int y, HostSuppliedSprite hostSuppliedSprite)
        {
            var monoGameSprite = (Texture2D) hostSuppliedSprite.HostObject;
            _spriteBatch.Draw(monoGameSprite, new Vector2(_originX + x, _originY + y), Color.White);
        }

        void IDrawingTarget.DrawSpritePieceStretched(
            int sx, int sy, int sw, int sh, 
            int dx, int dy, int dw, int dh, 
            HostSuppliedSprite hostSuppliedSprite)
        {
            var monoGameSprite = (Texture2D)hostSuppliedSprite.HostObject;

            _spriteBatch.Draw(
                monoGameSprite,
                new Rectangle(dx + _originX, dy + _originY, dw, dh),
                new Rectangle(sx, sy, sw, sh),
                Color.White);
        }
    }
}