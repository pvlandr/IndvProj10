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
    public abstract class LevelEditorTool_Object : LevelEditorTool
    {
        protected readonly List<Vector2Int> AffectedTiles = [];

        public abstract Texture2D Sprite { get; }

        public override void Draw(SpriteBatch batch, MouseState state, Vector2 mousePosition)
        {
            batch.Draw(Sprite, mousePosition, Color.White);
        }

        public override void Update(Grid grid, MouseState mouseState, Vector2Int mouseWorldPos)
        {
            if (mouseState.LeftButton == ButtonState.Pressed)
            {
                if (AffectedTiles.Contains(mouseWorldPos))
                    return;

                AffectedTiles.Add(mouseWorldPos);
                grid.Units.RemoveAll(x => x.Position == mouseWorldPos);

                var u = MakeUnit();
                u.Position = mouseWorldPos;

                grid.AddUnit(u, true);
            }

            else if (mouseState.RightButton == ButtonState.Pressed)
            {
                grid.Units.RemoveAll(x => x.Position == mouseWorldPos);
                grid.UpdateUnitMap();
            }

            if(mouseState.LeftButton == ButtonState.Released)
                AffectedTiles.Clear();
        }

        public abstract Unit MakeUnit();
    }
}
