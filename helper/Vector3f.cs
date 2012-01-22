using System;
using Microsoft.Xna.Framework;

namespace VoxelTest.helper
{
    using T = System.Single;

    public struct Vector3f
    {
        public T X;
        public T Y;
        public T Z;

        public Vector3f(T x, T y, T z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3f(T [] arr)
        {
            X = arr[0];
            Y = arr[1];
            Z = arr[2];
        }

        public Vector3i cast()
        {
            return new Vector3i((int)X,(int)Y,(int)Z);
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
            return new Vector3f((T)v.X, (T)v.Y, (T)v.Z);
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

        public static Vector3f operator +(Vector3f v, T s)
        {
            return new Vector3f(v.X + s,
                                    v.Y + s,
                                    v.Z + s);
        }

        public static Vector3f operator -(Vector3f v, T s)
        {
            return new Vector3f(v.X - s,
                                    v.Y - s,
                                    v.Z - s);
        }

        public static Vector3f operator -(T s, Vector3f v)
        {
            return new Vector3f(s - v.X,
                                    s - v.Y,
                                    s - v.Z);
        }

        public static Vector3f operator *(Vector3f v, T s)
        {
            return new Vector3f(v.X * s,
                                    v.Y * s,
                                    v.Z * s);
        }

        public static Vector3f operator /(Vector3f v, T s)
        {
            return new Vector3f(v.X / s,
                                    v.Y / s,
                                    v.Z / s);
        }

        public static Vector3f operator /(T s, Vector3f v)
        {
            return new Vector3f(s / v.X,
                                    s / v.Y,
                                    s / v.Z);
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