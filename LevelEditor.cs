using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IndvProj10
{
    public static class LevelEditor
    {
        public static int Mode = 0;
        public static readonly List<LevelEditorTool> Tools = new()
        {
            new LevelEditorTool_Tiles(),

            new LevelEditorTool_Object_Player(),
            new LevelEditorTool_Object_Box(),
            new LevelEditorTool_Object_Target(),
        };

        public static Grid LevelEditorGrid;
        public static int levelIdx;

        public static void Update()
        {
            var state = Mouse.GetState();
            var grid = Game.Instance.Grid;
            var mousePos = state.Position.ToVector2();
            var mouseWorldPos = grid.ViewportToWorld(mousePos);

            if (Game.Instance.GraphicsDevice.Viewport.Bounds.Contains(mousePos.X, mousePos.Y))
            {
                if (Mode < Tools.Count)
                    Tools[Mode].Update(grid, state, mouseWorldPos);
            }

            if (Input.HasBeenPressed(Keys.D1))
                Mode = 0;
            else if (Input.HasBeenPressed(Keys.D2))
                Mode = 1;
            else if (Input.HasBeenPressed(Keys.D3))
                Mode = 2;
            else if (Input.HasBeenPressed(Keys.D4))
                Mode = 3;
            else if (Input.HasBeenPressed(Keys.D5))
                Mode = 4;
            else if (Input.HasBeenPressed(Keys.D6))
                Mode = 5;
            else if (Input.HasBeenPressed(Keys.D7))
                Mode = 6;
            else if (Input.HasBeenPressed(Keys.D8))
                Mode = 7;
            else if (Input.HasBeenPressed(Keys.D9))
                Mode = 8;
            else if (Input.HasBeenPressed(Keys.D0))
                Mode = 9;

            if (Input.HasBeenPressed(Keys.Right))
                levelIdx++;
            else if(Input.HasBeenPressed(Keys.Left) && levelIdx > 0)
                levelIdx--;

            if(Input.IsPressed(Keys.LeftControl) && Input.HasBeenPressed(Keys.S))
            {
                grid.FixCoords();
                var bytes = LevelLoader.CompileLevel(grid);
                var filename = $"level{levelIdx:D3}";
                var dirPath = Path.Combine(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)))), "Content", "Levels");


                File.WriteAllBytes(Path.Combine(dirPath, filename), bytes);
            }
            else if (Input.IsPressed(Keys.LeftControl) && Input.HasBeenPressed(Keys.L))
            {
                var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream($"IndvProj10.Content.Levels.level{levelIdx:D3}");

                if(stream != null)
                    Game.Instance.Grid = grid = LevelLoader.LoadLevelFromStream(stream);
            }

            if (Input.HasBeenPressed(Keys.Enter))
            {
                grid.FixCoords();
                LevelEditorGrid = grid;

                Game.Instance.Grid = LevelEditorGrid.Clone();
                Game.IsInLevelEditor = false;
            }
        }

        public static void Draw(SpriteBatch batch)
        {
            var state = Mouse.GetState();
            var mousePos = state.Position.ToVector2();
            var grid = Game.Instance.Grid;

            batch.DrawString(Game.font, $"level{levelIdx:D3}", new(16, 16), Color.White);

            if (Game.Instance.GraphicsDevice.Viewport.Bounds.Contains(mousePos.X, mousePos.Y))
            {
                batch.Draw(Sprites.Grid, grid.WorldToViewport(grid.ViewportToWorld(mousePos)), null, Color.Red, 0f, Sprites.Grid.Bounds.Center.ToVector2(), 1f, SpriteEffects.None, 0f);

                if (Mode < Tools.Count)
                    Tools[Mode].Draw(batch, state, mousePos);
            }
        }
    }
}
