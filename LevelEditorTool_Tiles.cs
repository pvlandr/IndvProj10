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
    public class LevelEditorTool_Tiles : LevelEditorTool
    {
        public override void Draw(SpriteBatch batch, MouseState state, Vector2 mousePosition)
        {
            batch.Draw(Sprites.Grid, mousePosition, Color.White);
        }

        public override void Update(Grid grid, MouseState state, Vector2Int mouseWorldPos)
        {
            if (state.LeftButton == ButtonState.Pressed)
                grid.ValidTiles.Add(mouseWorldPos);

            else if(state.RightButton == ButtonState.Pressed)
                grid.ValidTiles.Remove(mouseWorldPos);
        }
    }
}
