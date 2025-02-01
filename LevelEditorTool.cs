using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndvProj10
{
    public abstract class LevelEditorTool
    {
        public abstract void Update(Grid grid, MouseState mouseState, Vector2Int mouseWorldPos);
        public abstract void Draw(SpriteBatch batch, MouseState state, Vector2 mousePosition);
    }
}
