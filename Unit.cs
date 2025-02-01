using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndvProj10
{
    public abstract class Unit
    {
        public Vector2Int Position;
        public Vector2 VisualPosition;

        public abstract Texture2D Sprite { get; }
        public virtual Color Color => Color.White;

        public virtual void Draw(SpriteBatch spriteBatch, Vector2 minPosition)
        {
            spriteBatch.Draw(Sprite, minPosition + VisualPosition, null, Color, 0f, Sprite.Bounds.Center.ToVector2(), 1f, SpriteEffects.None, 0f);
        }

        public virtual void VisualUpdate(float deltaTime)
        {
            var endPos = Position * Game.TileSize;
            
            if(VisualPosition != endPos)
            {
                var dist = (endPos - VisualPosition).LengthSquared();

                if(dist <= 1f)
                {
                    VisualPosition = endPos;
                }

                else
                {
                    VisualPosition = endPos + (VisualPosition - endPos) * MathF.Exp(-deltaTime * 25f);
                }
            }
        }

        public virtual void Command(Vector2Int direction)
        {
        }

        public virtual void CheckWinCondition(ref bool? canWin)
        {
        }

        public virtual PushBehavior GetPushBehavior()
        {
            return PushBehavior.Pass;
        }

        public abstract Unit Clone();

        public enum PushBehavior
        {
            Pass,
            Push,
            Stop
        }
    }
}
