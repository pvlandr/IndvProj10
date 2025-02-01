using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace IndvProj10
{
    public class Game : Microsoft.Xna.Framework.Game
    {
        public static Game Instance { get; private set; }

        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;

        public Grid Grid;
        public readonly Stack<Stack<UndoBase>> UndoStack = [];
        public Stack<UndoBase> CurrentUndo = [];

        public readonly List<Grid> Levels = [];
        public int level = 0;
        public bool win = false;

        public static float DeltaTime;
        public const int TileSize = 32;

        public const bool LEVELEDITORMODE = false;
        public static bool IsInLevelEditor = true;

        public static SpriteFont font;

        public Game()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            Instance = this;
        }

        protected override void Initialize()
        {
            base.Initialize();

            var w = 15;
            var h = 3;

            var tiles = new HashSet<Vector2Int>();

            for (var x = 0; x < w; x++)
            {
                for (var y = 0; y < h; y++)
                {
                    tiles.Add(new Vector2Int(x, y));
                }
            }

            var assembly = Assembly.GetExecutingAssembly();

            if (LEVELEDITORMODE)
            {
                Levels.Add(Grid = new Grid([new(0, 0)], []));
            }
            else
            {
                for(var i = 0; ; i++)
                {
                    var stream = assembly.GetManifestResourceStream($"IndvProj10.Content.Levels.level{i:D3}");

                    if (stream == null || stream.Length <= 0)
                        break;

                    Levels.Add(LevelLoader.LoadLevelFromStream(stream));
                }

                if (Levels.Count > 0)
                    Grid = Levels[0];
                else
                    Grid = new([new(0, 0)], []);
            }
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Sprites.Grid = Content.Load<Texture2D>("grid");
            Sprites.Square = Content.Load<Texture2D>("square");
            Sprites.Circle = Content.Load<Texture2D>("circle");
            Sprites.Target = Content.Load<Texture2D>("target");

            font = Content.Load<SpriteFont>("Arial");
        }

        protected override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Input.GetState();
            var dt = DeltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (IsActive)
            {
                if (Input.HasBeenPressed(Keys.Escape) && LEVELEDITORMODE && !IsInLevelEditor)
                    ExitToLevelEditor();

                if (LEVELEDITORMODE && IsInLevelEditor)
                    LevelEditor.Update();

                else if(Grid != null)
                {
                    if (Input.HasBeenPressed(Keys.R))
                        Restart();

                    else if (Input.HasBeenPressed(Keys.Z))
                        Undo();

                    else
                    {
                        var dir = GetInputDirection();

                        if (dir.X != 0 || dir.Y != 0)
                            Turn(dir);
                    }
                }
            }

            if(Grid != null)
            {
                foreach (var u in Grid.Units)
                {
                    u.VisualUpdate(dt);
                }
            }
        }

        public void Turn(Vector2Int dir)
        {
            foreach (var u in Grid.Units)
                u.Command(dir);

            Grid.UpdateUnitMap();

            var canWin = (bool?)null;
            foreach (var u in Grid.Units)
                u.CheckWinCondition(ref canWin);

            if (canWin == true)
                LevelWin();

            if (CurrentUndo.Count > 0)
            {
                UndoStack.Push(CurrentUndo);
                CurrentUndo = [];
            }
        }

        public void LevelWin()
        {
            if (LEVELEDITORMODE)
            {
                ExitToLevelEditor();
                return;
            }

            level++;
            UndoStack.Clear();

            if (level < Levels.Count)
                Grid = Levels[level];
            else
            {
                Grid = null;
                win = true;
            }
        }

        public void ExitToLevelEditor()
        {
            if(!LEVELEDITORMODE || IsInLevelEditor)
                return;

            UndoStack.Clear();
            Grid = LevelEditor.LevelEditorGrid ?? new([new(0, 0)], []);
            IsInLevelEditor = true;
        }

        public void Undo()
        {
            if (!UndoStack.TryPop(out var undoForTurn))
                return;

            while(undoForTurn.TryPop(out var und))
            {
                und.DoUndo();
            }
        }

        public void Restart()
        {
            while(UndoStack.TryPop(out var undoForTurn))
            {
                while (undoForTurn.TryPop(out var und))
                    und.DoUndo();
            }
        }

        public static Vector2Int GetInputDirection()
        {
            var x = 0;
            var y = 0;

            var upPressed = Input.HasBeenPressed(Keys.W);
            var downPressed = Input.HasBeenPressed(Keys.S);
            var leftPressed = Input.HasBeenPressed(Keys.A);
            var rightPressed = Input.HasBeenPressed(Keys.D);

            if (upPressed && !downPressed)
                y = -1;
            else if (downPressed && !upPressed)
                y = 1;

            if (rightPressed && !leftPressed)
                x = 1;
            else if (leftPressed && !rightPressed)
                x = -1;

            return new Vector2Int(x, y);
        }

        public bool TryMove(Unit unit, Vector2Int dir)
        {
            if (unit == null)
                return false;

            if(dir.X == 0 && dir.Y == 0)
                return false;

            var newPos = unit.Position + dir;

            if (!Grid.IsValidTile(newPos))
                return false;

            if (!Grid.TryGetUnitsAt(newPos, out var units))
            {
                CurrentUndo.Push(new Undo_Position(unit, unit.Position));
                unit.Position = newPos;

                return true;
            }

            foreach (var u in units)
            {
                var pushBehav = u.GetPushBehavior();

                if (pushBehav == Unit.PushBehavior.Pass)
                    continue;

                if (pushBehav == Unit.PushBehavior.Stop || (pushBehav == Unit.PushBehavior.Push && !TryMove(u, dir)))
                    return false;
            }

            CurrentUndo.Push(new Undo_Position(unit, unit.Position));
            unit.Position = newPos;

            return true;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            Grid?.Draw(_spriteBatch);
            if (LEVELEDITORMODE && IsInLevelEditor)
                LevelEditor.Draw(_spriteBatch);

            if (win)
            {
                var text = "Поздравляем!\nВы выиграли!";

                _spriteBatch.DrawString(font, text, GraphicsDevice.Viewport.Bounds.Center.ToVector2(), Color.White, 0f, font.MeasureString(text) / 2f, 1f, SpriteEffects.None, 0f);
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
