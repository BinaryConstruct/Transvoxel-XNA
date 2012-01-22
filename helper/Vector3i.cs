using System;
using Microsoft.Xna.Framework;

namespace VoxelStuff.helper
{
    using T = System.Int32;

    public struct Vector3i
    {
        public T X;
        public T Y;
        public T Z;

        public Vector3i(T x, T y, T z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3i(T [] arr)
        {
            X = arr[0];
            Y = arr[1];
            Z = arr[2];
        }

        public Vector3f cast()
        {
            return new Vector3f((float)X, (float)Y, (float)Z);
        }

        public T[] toArray()
        {
            return new T[3]{X,Y,Z};
        }

        public T this[T i]
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

        public static Vector3i UnitX = new Vector3i(1, 0, 0);
        public static Vector3i UnitY = new Vector3i(0, 1, 0);
        public static Vector3i UnitZ = new Vector3i(0, 0, 1);
        public static Vector3i Zero = new Vector3i(0, 0, 0);
        public static Vector3i One = new Vector3i(1, 1, 1);

        #region Operators

        public static explicit operator Vector3(Vector3i v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }

        public static explicit operator Vector3i(Vector3 v)
        {
            return new Vector3i((T)v.X, (T)v.Y, (T)v.Z);
        }

        public static Vector3i operator +(Vector3i v0, Vector3i v1)
        {
            return new Vector3i(v0.X + v1.X,
                                    v0.Y + v1.Y,
                                    v0.Z + v1.Z);
        }

        public static Vector3i operator -(Vector3i v0, Vector3i v1)
        {
            return new Vector3i(v0.X - v1.X,
                                    v0.Y - v1.Y,
                                    v0.Z - v1.Z);
        }
        public static Vector3i operator /(Vector3i v0, Vector3i v1)
        {
            return new Vector3i(v0.X / v1.X,
                                    v0.Y / v1.Y,
                                    v0.Z / v1.Z);
        }
        public static Vector3i operator *(Vector3i v0, Vector3i v1)
        {
            return new Vector3i(v0.X * v1.X,
                                    v0.Y * v1.Y,
                                    v0.Z * v1.Z);
        }

        public static Vector3i operator +(Vector3i v, T s)
        {
            return new Vector3i(v.X + s,
                                    v.Y + s,
                                    v.Z + s);
        }

        public static Vector3i operator -(Vector3i v, T s)
        {
            return new Vector3i(v.X - s,
                                    v.Y - s,
                                    v.Z - s);
        }

        public static Vector3i operator -(T s, Vector3i v)
        {
            return new Vector3i(s - v.X,
                                    s - v.Y,
                                    s - v.Z);
        }

        public static Vector3i operator *(Vector3i v, T s)
        {
            return new Vector3i(v.X * s,
                                    v.Y * s,
                                    v.Z * s);
        }

        public static Vector3i operator /(Vector3i v, T s)
        {
            return new Vector3i(v.X / s,
                                    v.Y / s,
                                    v.Z / s);
        }

        public static Vector3i operator /(T s, Vector3i v)
        {
            return new Vector3i(s / v.X,
                                    s / v.Y,
                                    s / v.Z);
        }

        public static bool operator <(Vector3i a, Vector3i b)
        {
            return a.X < b.X && a.Y < b.Y && a.Z < b.Z;
        }

        public static bool operator >(Vector3i a, Vector3i b)
        {
            return a.X > b.X && a.Y > b.Y && a.Z > b.Z;
        }

        public static bool operator <=(Vector3i a, Vector3i b)
        {
            return a.X <= b.X && a.Y <= b.Y && a.Z <= b.Z;
        }

        public static bool operator >=(Vector3i a, Vector3i b)
        {
            return a.X >= b.X && a.Y >= b.Y && a.Z >= b.Z;
        }

        #endregion

        #region Equality
        
        public bool Equals(Vector3i other)
        {
            return other.Y == Y && other.Z == Z && other.X == X;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof (Vector3i)) return false;
            return Equals((Vector3i) obj);
        }

        public override T GetHashCode()
        {
            unchecked
            {
                T result = Y;
                result = (result*397) ^ Z;
                result = (result*397) ^ X;
                return result;
            }
        }
        #endregion
    }
}