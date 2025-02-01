using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndvProj10
{
    public class Player : Unit
    {
        public override Texture2D Sprite => Sprites.Square;

        public override void Command(Vector2Int direction)
        {
            base.Command(direction);

            Game.Instance.TryMove(this, direction);
        }

        public override Unit Clone()
        {
            return new Player()
            {
                Position = Position,
            };
        }
    }
}
