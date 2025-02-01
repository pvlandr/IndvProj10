using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndvProj10
{
    public class LevelEditorTool_Object_Player : LevelEditorTool_Object
    {
        public override Texture2D Sprite => Sprites.Square;

        public override Unit MakeUnit()
        {
            return new Player();
        }
    }
}
