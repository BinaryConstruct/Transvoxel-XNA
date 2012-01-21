using System;
using Microsoft.Xna.Framework;

namespace VoxelTest
{
    public struct Vector3Int32
    {
        public int X;
        public int Y;
        public int Z;

        public Vector3Int32(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public int this[int i]
        {
            get
            {
                if (i == 0)
                    return X;
                if (i == 1)
                    return Y;
                if (i == 2)
                    return Z;

                throw new ArgumentOutOfRangeException(string.Format("There is no value at {0} index.", i));
            }
            set
            {
                if (i == 0)
                    X = value;
                if (i == 1)
                    Y = value;
                if (i == 2)
                    Z = value;

                throw new ArgumentOutOfRangeException(string.Format("There is no value at {0} index.", i));
            }
        }

        public static Vector3Int32 UnitX = new Vector3Int32(1, 0, 0);
        public static Vector3Int32 UnitY = new Vector3Int32(0, 1, 0);
        public static Vector3Int32 UnitZ = new Vector3Int32(0, 0, 1);
        public static Vector3Int32 Zero = new Vector3Int32(0, 0, 0);
        public static Vector3Int32 One = new Vector3Int32(1, 1, 1);

        #region Operators

        public static explicit operator Vector3(Vector3Int32 v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }

        public static explicit operator Vector3Int32(Vector3 v)
        {
            return new Vector3Int32((int)v.X, (int)v.Y, (int)v.Z);
        }

        public static Vector3Int32 operator +(Vector3Int32 v0, Vector3Int32 v1)
        {
            return new Vector3Int32(v0.X + v1.X,
                                    v0.Y + v1.Y,
                                    v0.Z + v1.Z);
        }

        public static Vector3Int32 operator -(Vector3Int32 v0, Vector3Int32 v1)
        {
            return new Vector3Int32(v0.X - v1.X,
                                    v0.Y - v1.Y,
                                    v0.Z - v1.Z);
        }
        public static Vector3Int32 operator /(Vector3Int32 v0, Vector3Int32 v1)
        {
            return new Vector3Int32(v0.X / v1.X,
                                    v0.Y / v1.Y,
                                    v0.Z / v1.Z);
        }
        public static Vector3Int32 operator *(Vector3Int32 v0, Vector3Int32 v1)
        {
            return new Vector3Int32(v0.X * v1.X,
                                    v0.Y * v1.Y,
                                    v0.Z * v1.Z);
        }

        public static Vector3Int32 operator +(Vector3Int32 v, int s)
        {
            return new Vector3Int32(v.X + s,
                                    v.Y + s,
                                    v.Z + s);
        }

        public static Vector3Int32 operator -(Vector3Int32 v, int s)
        {
            return new Vector3Int32(v.X - s,
                                    v.Y - s,
                                    v.Z - s);
        }

        public static Vector3Int32 operator -(int s, Vector3Int32 v)
        {
            return new Vector3Int32(s - v.X,
                                    s - v.Y,
                                    s - v.Z);
        }

        public static Vector3Int32 operator *(Vector3Int32 v, int s)
        {
            return new Vector3Int32(v.X * s,
                                    v.Y * s,
                                    v.Z * s);
        }

        public static Vector3Int32 operator /(Vector3Int32 v, int s)
        {
            return new Vector3Int32(v.X / s,
                                    v.Y / s,
                                    v.Z / s);
        }

        public static Vector3Int32 operator /(int s, Vector3Int32 v)
        {
            return new Vector3Int32(s / v.X,
                                    s / v.Y,
                                    s / v.Z);
        }

        #endregion

        #region Equality
        
        public bool Equals(Vector3Int32 other)
        {
            return other.Y == Y && other.Z == Z && other.X == X;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (Vector3Int32)) return false;
            return Equals((Vector3Int32) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = Y;
                result = (result*397) ^ Z;
                result = (result*397) ^ X;
                return result;
            }
        }
        #endregion
    }
}