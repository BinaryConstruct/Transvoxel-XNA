namespace TransvoxelXnaStudio.TransvoxelHelpers
{
    using System;
    using Microsoft.Xna.Framework;

    public struct FastFrustum
    {
        private readonly Vector3 _nearNormal, _leftNormal, _rightNormal, _bottomNormal, _topNormal;
        private readonly float _nearD, _leftD, _rightD, _bottomD, _topD;

        public FastFrustum(BoundingFrustum source)
        {

            _nearNormal = source.Near.Normal; _nearD = source.Near.D;
            _leftNormal = source.Left.Normal; _leftD = source.Left.D;
            _rightNormal = source.Right.Normal; _rightD = source.Right.D;
            _bottomNormal = source.Bottom.Normal; _bottomD = source.Bottom.D;
            _topNormal = source.Top.Normal; _topD = source.Top.D;
            /* FarNormal    = source.Far.Normal;    FarD    = source.Far.D; */

        }

        // Camera matrix is view * projection 
        public FastFrustum(ref Matrix cameraMatrix)
        {

            float x = -cameraMatrix.M14 - cameraMatrix.M11;
            float y = -cameraMatrix.M24 - cameraMatrix.M21;
            float z = -cameraMatrix.M34 - cameraMatrix.M31;
            float scale = 1.0f / ((float)Math.Sqrt((x * x) + (y * y) + (z * z)));
            _leftNormal = new Vector3(x * scale, y * scale, z * scale);
            _leftD = (-cameraMatrix.M44 - cameraMatrix.M41) * scale;

            x = -cameraMatrix.M14 + cameraMatrix.M11;
            y = -cameraMatrix.M24 + cameraMatrix.M21;
            z = -cameraMatrix.M34 + cameraMatrix.M31;
            scale = 1.0f / ((float)Math.Sqrt((x * x) + (y * y) + (z * z)));
            _rightNormal = new Vector3(x * scale, y * scale, z * scale);
            _rightD = (-cameraMatrix.M44 + cameraMatrix.M41) * scale;

            x = -cameraMatrix.M14 + cameraMatrix.M12;
            y = -cameraMatrix.M24 + cameraMatrix.M22;
            z = -cameraMatrix.M34 + cameraMatrix.M32;
            scale = 1.0f / ((float)Math.Sqrt((x * x) + (y * y) + (z * z)));
            _topNormal = new Vector3(x * scale, y * scale, z * scale);
            _topD = (-cameraMatrix.M44 + cameraMatrix.M42) * scale;

            x = -cameraMatrix.M14 - cameraMatrix.M12;
            y = -cameraMatrix.M24 - cameraMatrix.M22;
            z = -cameraMatrix.M34 - cameraMatrix.M32;
            scale = 1.0f / ((float)Math.Sqrt((x * x) + (y * y) + (z * z)));
            _bottomNormal = new Vector3(x * scale, y * scale, z * scale);
            _bottomD = (-cameraMatrix.M44 - cameraMatrix.M42) * scale;

            x = -cameraMatrix.M13;
            y = -cameraMatrix.M23;
            z = -cameraMatrix.M33;
            scale = 1.0f / ((float)Math.Sqrt((x * x) + (y * y) + (z * z)));
            _nearNormal = new Vector3(x * scale, y * scale, z * scale);
            _nearD = (-cameraMatrix.M43) * scale;

            /*z = -cameraMatrix.M14 + cameraMatrix.M13;
              y = -cameraMatrix.M24 + cameraMatrix.M23;
              z = -cameraMatrix.M34 + cameraMatrix.M33;
              scale = 1.0f / ((float) Math.Sqrt((x * x) + (y * y) + (z * z)));
              FarNormal = new Vector3(x * scale, y * scale, z * scale);
              FarD      = (-cameraMatrix.M44 + cameraMatrix.M43) * scale;*/

        }

        public bool Intersects(BoundingSphere sphere)
        {

            Vector3 p = sphere.Center; float radius = sphere.Radius;

            if (_nearD + (_nearNormal.X * p.X) + (_nearNormal.Y * p.Y) + (_nearNormal.Z * p.Z) > radius) return false;
            if (_leftD + (_leftNormal.X * p.X) + (_leftNormal.Y * p.Y) + (_leftNormal.Z * p.Z) > radius) return false;
            if (_rightD + (_rightNormal.X * p.X) + (_rightNormal.Y * p.Y) + (_rightNormal.Z * p.Z) > radius) return false;
            if (_bottomD + (_bottomNormal.X * p.X) + (_bottomNormal.Y * p.Y) + (_bottomNormal.Z * p.Z) > radius) return false;
            if (_topD + (_topNormal.X * p.X) + (_topNormal.Y * p.Y) + (_topNormal.Z * p.Z) > radius) return false;

            /* Can ignore far plane when distant object culling is handled by another mechanism
            if(FarD + (FarNormal.X * p.X) + (FarNormal.Y * p.Y) + (FarNormal.Z * p.Z) > radius)             return false;
            */
            return true;

        }

        public bool Intersects(BoundingBox box)
        {

            Vector3 p;

            p.X = _nearNormal.X >= 0 ? box.Min.X : box.Max.X;
            p.Y = _nearNormal.Y >= 0 ? box.Min.Y : box.Max.Y;
            p.Z = _nearNormal.Z >= 0 ? box.Min.Z : box.Max.Z;
            if (_nearD + (_nearNormal.X * p.X) + (_nearNormal.Y * p.Y) + (_nearNormal.Z * p.Z) > 0) return false;

            p.X = _leftNormal.X >= 0 ? box.Min.X : box.Max.X;
            p.Y = _leftNormal.Y >= 0 ? box.Min.Y : box.Max.Y;
            p.Z = _leftNormal.Z >= 0 ? box.Min.Z : box.Max.Z;
            if (_leftD + (_leftNormal.X * p.X) + (_leftNormal.Y * p.Y) + (_leftNormal.Z * p.Z) > 0) return false;

            p.X = _rightNormal.X >= 0 ? box.Min.X : box.Max.X;
            p.Y = _rightNormal.Y >= 0 ? box.Min.Y : box.Max.Y;
            p.Z = _rightNormal.Z >= 0 ? box.Min.Z : box.Max.Z;
            if (_rightD + (_rightNormal.X * p.X) + (_rightNormal.Y * p.Y) + (_rightNormal.Z * p.Z) > 0) return false;

            p.X = _bottomNormal.X >= 0 ? box.Min.X : box.Max.X;
            p.Y = _bottomNormal.Y >= 0 ? box.Min.Y : box.Max.Y;
            p.Z = _bottomNormal.Z >= 0 ? box.Min.Z : box.Max.Z;
            if (_bottomD + (_bottomNormal.X * p.X) + (_bottomNormal.Y * p.Y) + (_bottomNormal.Z * p.Z) > 0) return false;

            p.X = _topNormal.X >= 0 ? box.Min.X : box.Max.X;
            p.Y = _topNormal.Y >= 0 ? box.Min.Y : box.Max.Y;
            p.Z = (_topNormal.Z >= 0 ? box.Min.Z : box.Max.Z);
            if (_topD + (_topNormal.X * p.X) + (_topNormal.Y * p.Y) + (_topNormal.Z * p.Z) > 0) return false;

            /* Can ignore far plane when distant object culling is handled by another mechanism
            p.X = (FarNormal.X >= 0 ? box.Min.X : box.Max.X);
            p.Y = (FarNormal.Y >= 0 ? box.Min.Y : box.Max.Y);
            p.Z = (FarNormal.Z >= 0 ? box.Min.Z : box.Max.Z);
            if(FarD + (FarNormal.X * p.X) + (FarNormal.Y * p.Y) + (FarNormal.Z * p.Z) > 0) return false;
             */

            return true;

        }

    }
}