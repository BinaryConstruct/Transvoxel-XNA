using System;
using Microsoft.Xna.Framework;

namespace TransvoxelXna.helper
{
    public struct Vector3f
    {
        public float X;
        public float Y;
        public float Z;

        public Vector3f(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3f(float [] arr)
        {
            X = arr[0];
            Y = arr[1];
            Z = arr[2];
        }

        public Vector3i cast()
        {
            return new Vector3i((int)X,(int)Y,(int)Z);
        }

        public float[] toArray()
        {
            return new float[3]{X,Y,Z};
        }

        public float this[float i]
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

        public static Vector3f UnitX = new Vector3f(1, 0, 0);
        public static Vector3f UnitY = new Vector3f(0, 1, 0);
        public static Vector3f UnitZ = new Vector3f(0, 0, 1);
        public static Vector3f Zero = new Vector3f(0, 0, 0);
        public static Vector3f One = new Vector3f(1, 1, 1);

        #region Operators

        public static explicit operator Vector3(Vector3f v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }

        public static explicit operator Vector3f(Vector3 v)
        {
            return new Vector3f((float)v.X, (float)v.Y, (float)v.Z);
        }

        public static Vector3f operator +(Vector3f v0, Vector3f v1)
        {
            return new Vector3f(v0.X + v1.X,
                                    v0.Y + v1.Y,
                                    v0.Z + v1.Z);
        }

        public static Vector3f operator -(Vector3f v0, Vector3f v1)
        {
            return new Vector3f(v0.X - v1.X,
                                    v0.Y - v1.Y,
                                    v0.Z - v1.Z);
        }
        public static Vector3f operator /(Vector3f v0, Vector3f v1)
        {
            return new Vector3f(v0.X / v1.X,
                                    v0.Y / v1.Y,
                                    v0.Z / v1.Z);
        }
        public static Vector3f operator *(Vector3f v0, Vector3f v1)
        {
            return new Vector3f(v0.X * v1.X,
                                    v0.Y * v1.Y,
                                    v0.Z * v1.Z);
        }

        public static Vector3f operator +(Vector3f v, float s)
        {
            return new Vector3f(v.X + s,
                                    v.Y + s,
                                    v.Z + s);
        }

        public static Vector3f operator -(Vector3f v, float s)
        {
            return new Vector3f(v.X - s,
                                    v.Y - s,
                                    v.Z - s);
        }

        public static Vector3f operator *(Vector3f v, float s)
        {
            return new Vector3f(v.X * s,
                                    v.Y * s,
                                    v.Z * s);
        }

        public static Vector3f operator /(Vector3f v, float s)
        {
            return new Vector3f(v.X / s,
                                    v.Y / s,
                                    v.Z / s);
        }

        public static bool operator <(Vector3f a, Vector3f b)
        {
            return a.X < b.X && a.Y < b.Y && a.Z < b.Z;
        }

        public static bool operator >(Vector3f a, Vector3f b)
        {
            return a.X > b.X && a.Y > b.Y && a.Z > b.Z;
        }

        public static bool operator <=(Vector3f a, Vector3f b)
        {
            return a.X <= b.X && a.Y <= b.Y && a.Z <= b.Z;
        }

        public static bool operator >=(Vector3f a, Vector3f b)
        {
            return a.X >= b.X && a.Y >= b.Y && a.Z >= b.Z;
        }

        #endregion

        #region Equality
        
        public bool Equals(Vector3f other)
        {
            return other.Y == Y && other.Z == Z && other.X == X;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (Vector3f)) return false;
            return Equals((Vector3f) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (int)Y;
                result = (result*397) ^ (int)Z;
                result = (result*397) ^ (int)X;
                return result;
            }
        }
        #endregion
    }
}