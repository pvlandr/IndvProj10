using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndvProj10
{
    public static class LevelLoader
    {
        public const byte NOP = 0;
        public const byte PL_GRID = 1;
        public const byte MVDOWN = 2;
        public const byte MVLEFT = 3;
        public const byte PL_O_PLR = 4;
        public const byte PL_O_BOX = 5;
        public const byte PL_O_TRG = 6;

        public static Grid LoadLevelFromStream(Stream stream)
        {
            using BinaryReader reader = new BinaryReader(stream);

            var tiles = new HashSet<Vector2Int>();
            var units = new List<Unit>();

            var x = 0;
            var y = 0;

            while (stream.Position < reader.BaseStream.Length)
            {
                var b = reader.ReadByte();

                switch (b)
                {
                    case NOP:
                        break;

                    case PL_GRID:
                        tiles.Add(new Vector2Int(x, y));
                        break;

                    case MVDOWN:
                        x = -1;
                        y++;
                        break;

                    case MVLEFT:
                        x -= 2;
                        break;

                    case PL_O_PLR:
                        units.Add(new Player() { Position = new(x, y) });
                        break;

                    case PL_O_BOX:
                        units.Add(new Box() { Position = new(x, y) });
                        break;

                    case PL_O_TRG:
                        units.Add(new Target() { Position = new(x, y) });
                        break;
                }

                x++;
            }

            var lvl = new Grid(tiles, units);
            return lvl;
        }

        public static byte[] CompileLevel(Grid level)
        {
            var w = level.Width;
            var h = level.Height;
            var ba = new List<byte>();

            for(var y = 0; y < h; y++)
            {
                for(var x = 0; x < w; x++)
                {
                    var pos = new Vector2Int(x, y);
                    var tileHere = level.ValidTiles.Contains(pos);
                    var unitsHere = level.TryGetUnitsAt(pos, out var u);

                    if (!tileHere && !unitsHere)
                    {
                        ba.Add(NOP);
                        continue;
                    }

                    if (tileHere)
                    {
                        ba.Add(PL_GRID);

                        if (unitsHere)
                            ba.Add(MVLEFT);
                    }

                    if (unitsHere)
                    {
                        for(var i = 0; i < u.Count; i++)
                        {
                            var un = u[i];

                            if (un is Player)
                                ba.Add(PL_O_PLR);
                            else if (un is Box)
                                ba.Add(PL_O_BOX);
                            else if (un is Target)
                                ba.Add(PL_O_TRG);

                            if (i < u.Count - 1)
                                ba.Add(MVLEFT);
                        }
                    }
                }

                for(var i = ba.Count - 1; i >= 0 && ba[i] == 0; i--)
                {
                    ba.RemoveAt(i);
                }

                if(y < h - 1)
                    ba.Add(MVDOWN);
            }

            return [.. ba];
        }
    }
}
