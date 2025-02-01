using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndvProj10
{
    public class Target : Unit
    {
        public override Texture2D Sprite => Sprites.Target;

        public override void CheckWinCondition(ref bool? canWin)
        {
            if (canWin == false)
                return;

            canWin = Game.Instance.Grid.TryGetUnitsAt(Position, out var here) && here.Count > 1;
        }

        public override Unit Clone()
        {
            return new Target()
            {
                Position = Position
            };
        }
    }
}
