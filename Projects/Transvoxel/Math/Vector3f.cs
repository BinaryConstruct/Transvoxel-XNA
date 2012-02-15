// some good info here 
// http://www.codeproject.com/Articles/17425/A-Vector-Type-for-C
using System;

namespace Transvoxel.Math
{
    public struct Vector3f
    {
        //public const double EqualityTolerence = Double.Epsilon;

        public float X;
        public float Y;
        public float Z;

        public Vector3f(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3f(float[] arr)
        {
            X = arr[0];
            Y = arr[1];
            Z = arr[2];
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

        public string ToString()
        {
            return string.Format("({0}, {1}, {2})", X, Y, Z);
        }

        public static Vector3f UnitX = new Vector3f(1, 0, 0);
        public static Vector3f UnitY = new Vector3f(0, 1, 0);
        public static Vector3f UnitZ = new Vector3f(0, 0, 1);
        public static Vector3f Zero = new Vector3f(0, 0, 0);
        public static Vector3f One = new Vector3f(1, 1, 1);

        #region Vector Math

        /// <summary>
        /// Use MagnitudeSquare whenever possible to avoid expensive Math.Sqrt
        /// </summary>
        public float MagnitudeSquare
        {
            get { return X * X + Y * Y + Z * Z; }
        }
        public float Magnitude
        {
            get { return (float)System.Math.Sqrt(MagnitudeSquare); }
        }

        public static float DistanceSquared(Vector3f v1, Vector3f v2)
        {
            return (v1.X - v2.X)*(v1.X - v2.X) +
                   (v1.Y - v2.Y)*(v1.Y - v2.Y) +
                   (v1.Z - v2.Z)*(v1.Z - v2.Z);
        }
        public float DistanceSquared(Vector3f v)
        {
            return DistanceSquared(this, v);
        }
        public static float Distance(Vector3f v1, Vector3f v2)
        {
            return (float)System.Math.Sqrt(DistanceSquared(v1, v2));
        }
        public float Distance(Vector3f v)
        {
            return DistanceSquared(this, v);
        }

        public static double Angle(Vector3f v1, Vector3f v2)
        {
           return
           (
              System.Math.Acos
              (
                 Normalize(v1).Dot(Normalize(v2))
              )
           );
        }

        public double Angle(Vector3f other)
        {
            return Angle(this, other);
        }

        public static Vector3f Max(Vector3f v1, Vector3f v2)
        {
            if (v1 >= v2) { return v1; }
            return v2;
        }

        public Vector3f Max(Vector3f other)
        {
            return Max(this, other);
        }

        public static Vector3f Min(Vector3f v1, Vector3f v2)
        {
            if (v1 <= v2) { return v1; }
            return v2;
        }

        public Vector3f Min(Vector3f other)
        {
            return Min(this, other);
        }

        public static bool IsUnitVector(Vector3f v)
        {
            return v.MagnitudeSquare == 1;
        }
        public bool IsUnitVector()
        {
            return IsUnitVector(this);
        }

        public void Normalize()
        {
            this = Normalize(this);
        }
        public static Vector3f Normalize(Vector3f v)
        {
            if (v.MagnitudeSquare == 0)
                throw new DivideByZeroException("Normalize_0");


            float inverse = 1 / v.Magnitude;
            return new Vector3f(
                v.X * inverse,
                v.Y * inverse,
                v.Z * inverse);
        }

        public static Vector3f Cross(Vector3f v1, Vector3f v2)
        {
            return
            (
               new Vector3f
               (
                  v1.Y * v2.Z - v1.Z * v2.Y,
                  v1.Z * v2.X - v1.X * v2.Z,
                  v1.X * v2.Y - v1.Y * v2.X
               )
            );
        }

        public static float Dot(Vector3f v1, Vector3f v2)
        {
            return
            (
               v1.X * v2.X +
               v1.Y * v2.Y +
               v1.Z * v2.Z
            );
        }
        public float Dot(Vector3f v)
        {
            return Dot(this, v);
        }

        public static Vector3f Lerp(Vector3f v1, Vector3f v2, float control)
        {
            if (control > 1 || control < 0)
            {
                // Error message includes information about the actual value of the 
                // argument
                throw new ArgumentOutOfRangeException("control", "Must be between 0 and 1");
            }

            return
            (
               new Vector3f
               (
                   v1.X * (1 - control) + v2.X * control,
                   v1.Y * (1 - control) + v2.Y * control,
                   v1.Z * (1 - control) + v2.Z * control
                )
            );

        }
        public Vector3f Lerp(Vector3f other, float control)
        {
            return Lerp(this, other, control);
        }

        #endregion

        #region Operators

        public static explicit operator Vector3f(Vector3i v)
        {
            return new Vector3f(v.X, v.Y, v.Z);
        }


        public static Vector3f operator +(Vector3f v0, Vector3f v1)
        {
            return new Vector3f(v0.X + v1.X,
                                    v0.Y + v1.Y,
                                    v0.Z + v1.Z);
        }
        public void Add(Vector3f v)
        {
            this += v;
        }

        public static Vector3f operator -(Vector3f v0, Vector3f v1)
        {
            return new Vector3f(v0.X - v1.X,
                                    v0.Y - v1.Y,
                                    v0.Z - v1.Z);
        }
        public void Sub(Vector3f v)
        {
            this -= v;
        }

        public static Vector3f operator /(Vector3f v0, Vector3f v1)
        {
            return new Vector3f(v0.X / v1.X,
                                    v0.Y / v1.Y,
                                    v0.Z / v1.Z);
        }
        public void Div(Vector3f v)
        {
            this /= v;
        }

        public static Vector3f operator *(Vector3f v0, Vector3f v1)
        {
            return new Vector3f(v0.X * v1.X,
                                    v0.Y * v1.Y,
                                    v0.Z * v1.Z);
        }
        public void Mul(Vector3f v)
        {
            this *= v;
        }

        public static Vector3f operator *(Vector3f v, float s)
        {
            return new Vector3f(v.X * s,
                                    v.Y * s,
                                    v.Z * s);
        }
        public static Vector3f operator *(float s, Vector3f v)
        {
            return v * s;
        }
        public void Mul(float s)
        {
            this *= s;
        }

        public static Vector3f operator /(Vector3f v, float s)
        {
            return new Vector3f(v.X / s,
                                    v.Y / s,
                                    v.Z / s);
        }
        public void Div(float s)
        {
            this /= s;
        }

        public static bool operator <(Vector3f a, Vector3f b)
        {
            return a.MagnitudeSquare < b.MagnitudeSquare;
        }

        public static bool operator >(Vector3f a, Vector3f b)
        {
            return a.MagnitudeSquare > b.MagnitudeSquare;
        }

        public static bool operator <=(Vector3f a, Vector3f b)
        {
            return a.MagnitudeSquare <= b.MagnitudeSquare;
        }

        public static bool operator >=(Vector3f a, Vector3f b)
        {
            return a.MagnitudeSquare >= b.MagnitudeSquare;
        }

        #endregion

        #region Equality

        public bool Equals(Vector3f other)
        {
            return other.Y.Equals(Y) && other.Z.Equals(Z) && other.X.Equals(X);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (obj.GetType() != typeof(Vector3f)) return false;
            return Equals((Vector3f)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int result = (int)Y;
                result = (result * 397) ^ (int)Z;
                result = (result * 397) ^ (int)X;
                return result;
            }
        }
        #endregion
    }
}