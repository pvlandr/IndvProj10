using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndvProj10
{
    public static class VectorExtensions
    {
        public static Vector2Int RoundToInt(this Vector2 vec)
        {
            return new((int)MathF.Round(vec.X), (int)MathF.Round(vec.Y));
        }
    }
}
