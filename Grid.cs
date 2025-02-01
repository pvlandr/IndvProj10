using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndvProj10
{
    public class Grid
    {
        public readonly HashSet<Vector2Int> ValidTiles;
        public readonly List<Unit> Units;

        public readonly Dictionary<Vector2Int, List<Unit>> UnitMap;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public Grid(HashSet<Vector2Int> validTiles, List<Unit> units)
        {
            ValidTiles = validTiles;

            foreach (var tile in ValidTiles)
            {
                Width = Math.Max(Width, tile.X + 1);
                Height = Math.Max(Height, tile.Y + 1);
            }

            Units = units;

            UnitMap = [];
            UpdateUnitMap();
        }

        public void AddUnit(Unit u, bool updateMap = true)
        {
            Units.Add(u);

            if (updateMap)
                UpdateUnitMap();
        }

        public bool IsValidTile(Vector2Int tile)
        {
            return ValidTiles.Contains(tile);
        }

        public void UpdateUnitMap()
        {
            UnitMap.Clear();

            foreach(var u in Units)
            {
                if(!UnitMap.TryGetValue(u.Position, out var unitsHere))
                    UnitMap[u.Position] = unitsHere = [];

                unitsHere.Add(u);
            }
        }

        public bool TryGetUnitsAt(Vector2Int position, out List<Unit> unitsHere)
        {
            return UnitMap.TryGetValue(position, out unitsHere);
        }

        public void Draw(SpriteBatch batch)
        {
            var screenMin = ScreenMinPosition;

            foreach (var t in ValidTiles)
            {
                batch.Draw(Sprites.Grid, screenMin + (t * Game.TileSize), null, Color.Gray, 0f, Sprites.Grid.Bounds.Center.ToVector2(), 1f, SpriteEffects.None, 0);
            }

            foreach (var u in Units)
            {
                u.Draw(batch, screenMin);
            }
        }

        public Vector2 WorldToViewport(Vector2Int world)
        {
            return ScreenMinPosition + (world * Game.TileSize);
        }

        public Vector2Int ViewportToWorld(Vector2 viewport)
        {
            return ((viewport - ScreenMinPosition) / 32f).RoundToInt();
        }

        public void FixCoords()
        {
            var minX = int.MaxValue;
            var minY = int.MaxValue;

            var maxX = int.MinValue;
            var maxY = int.MinValue;

            foreach(var t in ValidTiles)
            {
                minX = Math.Min(minX, t.X);
                minY = Math.Min(minY, t.Y);
                maxX = Math.Max(maxX, t.X);
                maxY = Math.Max(maxY, t.Y);
            }

            foreach(var u in Units)
            {
                minX = Math.Min(minX, u.Position.X);
                minY = Math.Min(minY, u.Position.Y);
                maxX = Math.Max(maxX, u.Position.X);
                maxY = Math.Max(maxY, u.Position.Y);
            }

            Width = maxX - minX + 1;
            Height = maxY - minY + 1;

            if (minX == 0 && minY == 0)
                return;

            var shift = -new Vector2Int(minX, minY);

            var validTiles = ValidTiles.ToArray();
            ValidTiles.Clear();

            foreach(var t in validTiles)
                ValidTiles.Add(t + shift);

            foreach(var u in Units)
            {
                u.Position += shift;
            }

            UpdateUnitMap();
        }

        public Grid Clone()
        {
            return new Grid([.. ValidTiles], [.. Units.Select(x => x.Clone())]);
        }

        public Vector2 ScreenMinPosition => Game.Instance.GraphicsDevice.Viewport.Bounds.Center.ToVector2() + CenterOffset;
        public Vector2 CenterOffset => -(new Vector2(Width - 1, Height - 1) * Game.TileSize / 2f);
    }
}
