using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace IndvProj10
{
    public struct Vector2Int
    {
        public int X;
        public int Y;

        public Vector2Int()
        {
            X = 0;
            Y = 0;
        }

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator Vector2(Vector2Int a)
        {
            return new(a.X, a.Y);
        }

        public static Vector2Int operator -(Vector2Int value)
        {
            value.X = 0 - value.X;
            value.Y = 0 - value.Y;

            return value;
        }

        public static Vector2Int operator +(Vector2Int value1, Vector2Int value2)
        {
            value1.X += value2.X;
            value1.Y += value2.Y;

            return value1;
        }

        public static Vector2Int operator -(Vector2Int value1, Vector2Int value2)
        {
            value1.X -= value2.X;
            value1.Y -= value2.Y;

            return value1;
        }

        public static Vector2Int operator *(Vector2Int value1, int scale)
        {
            value1.X *= scale;
            value1.Y *= scale;

            return value1;
        }

        public static Vector2Int operator *(Vector2Int value1, Vector2Int value2)
        {
            value1.X *= value2.X;
            value1.Y *= value2.Y;

            return value1;
        }

        public static bool operator ==(Vector2Int value1, Vector2Int value2)
        {
            if (value1.X == value2.X)
            {
                return value1.Y == value2.Y;
            }

            return false;
        }

        public static bool operator !=(Vector2Int value1, Vector2Int value2)
        {
            if (value1.X == value2.X)
            {
                return value1.Y != value2.Y;
            }

            return true;
        }

        public override bool Equals(object obj)
        {
            if (obj is Vector2Int vector)
            {
                return Equals(vector);
            }

            return false;
        }

        public bool Equals(Vector2Int other)
        {
            if (X == other.X)
                return Y == other.Y;

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(X, Y);
        }

        public override string ToString()
        {
            return $"({X}, {Y})";
        }
    }
}
