using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace VoxelTest
{
    public struct VertexVoxel : IVertexType
    {
        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TextureCoordinateXY;
        public Vector3 TangentXY;
        public Vector3 BinormalXY;
        public Vector2 TextureCoordinateXZ;
        public Vector3 TangentXZ;
        public Vector3 BinormalXZ;

        public static int SizeInBytes
        {
            get { return 12 * 6 + 8 * 2; }
        }

        public readonly static VertexElement[] VertexElements =
            {
                new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
                new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
                new VertexElement(24, VertexElementFormat.Vector2,VertexElementUsage.TextureCoordinate, 0),
                new VertexElement(32, VertexElementFormat.Vector3, VertexElementUsage.Tangent, 0),
                new VertexElement(44, VertexElementFormat.Vector3, VertexElementUsage.Binormal, 0),
                new VertexElement(56, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 1),
                new VertexElement(64, VertexElementFormat.Vector3, VertexElementUsage.Tangent, 1),
                new VertexElement(76, VertexElementFormat.Vector3, VertexElementUsage.Binormal, 1),
            };

        #region Implementation of IVertexType

        public static readonly VertexDeclaration VertexDeclaration = new VertexDeclaration(VertexElements);
        VertexDeclaration IVertexType.VertexDeclaration { get { return VertexDeclaration; } }

        #endregion
    }

    public static class VertexProcessor
    {
        public static void BuildTangentSpaceDataForTriangleList(short[] indices, VertexVoxel[] vertices)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Normal = new Vector3(0, 0, 0);
                vertices[i].TangentXY = new Vector3(0, 0, 0);
                vertices[i].BinormalXY = new Vector3(0, 0, 0);
                vertices[i].TangentXZ = new Vector3(0, 0, 0);
                vertices[i].BinormalXZ = new Vector3(0, 0, 0);
            }

            for (int i = 0; i < indices.Length; i += 3)
            {
                int index_vert0 = indices[i];
                int index_vert1 = indices[i + 1];
                int index_vert2 = indices[i + 2];

                Vector3 firstvec = vertices[index_vert1].Position - vertices[index_vert0].Position;
                Vector3 secondvec = vertices[index_vert0].Position - vertices[index_vert2].Position;
                Vector3 normal = Vector3.Cross(firstvec, secondvec);
                normal.Normalize();
                vertices[index_vert0].Normal += normal;
                vertices[index_vert1].Normal += normal;
                vertices[index_vert2].Normal += normal;

                // Create XY Tangent and Binormal
                Vector2 uv0 = vertices[index_vert0].TextureCoordinateXY;
                Vector2 uv1 = vertices[index_vert1].TextureCoordinateXY;
                Vector2 uv2 = vertices[index_vert2].TextureCoordinateXY;

                float s1 = uv1.X - uv0.X;
                float s2 = uv2.X - uv0.X;
                float t1 = uv1.Y - uv0.Y;
                float t2 = uv2.Y - uv0.Y;

                float r = s1*t2 - s2*t1;
                if (r != 0.0f)
                {
                    r = 1.0f/r;

                    
                    Vector3 position0 = vertices[index_vert0].Position;
                    Vector3 position1 = vertices[index_vert1].Position;
                    Vector3 position2 = vertices[index_vert2].Position;

                    float x1 = position1.X - position0.X;
                    float x2 = position2.X - position0.X;
                    float y1 = position1.Y - position0.Y;
                    float y2 = position2.Y - position0.Y;
                    float z1 = position1.Z - position0.Z;
                    float z2 = position2.Z - position0.Z;

                    Vector3 tangent = new Vector3(
                        (t2*x1 - t1*x2)*r,
                        (t2*y1 - t1*y2)*r,
                        (t2*z1 - t1*z2)*r);

                    Vector3 binormal = new Vector3(
                        (s1*x2 - s2*x1)*r,
                        (s1*y2 - s2*y1)*r,
                        (s1*z2 - s2*z1)*r);

                    vertices[index_vert0].TangentXY += tangent;
                    vertices[index_vert1].TangentXY += tangent;
                    vertices[index_vert2].TangentXY += tangent;

                    vertices[index_vert0].BinormalXY += binormal;
                    vertices[index_vert1].BinormalXY += binormal;
                    vertices[index_vert2].BinormalXY += binormal;
                }

                // Create ZZ Tangent and Binormal
                uv0 = vertices[index_vert0].TextureCoordinateXZ;
                uv1 = vertices[index_vert1].TextureCoordinateXZ;
                uv2 = vertices[index_vert2].TextureCoordinateXZ;

                s1 = uv1.X - uv0.X;
                s2 = uv2.X - uv0.X;
                t1 = uv1.Y - uv0.Y;
                t2 = uv2.Y - uv0.Y;

                r = s1 * t2 - s2 * t1;
                if (r != 0.0f)
                {
                    r = 1.0f / r;


                    Vector3 position0 = vertices[index_vert0].Position;
                    Vector3 position1 = vertices[index_vert1].Position;
                    Vector3 position2 = vertices[index_vert2].Position;

                    float x1 = position1.X - position0.X;
                    float x2 = position2.X - position0.X;
                    float y1 = position1.Y - position0.Y;
                    float y2 = position2.Y - position0.Y;
                    float z1 = position1.Z - position0.Z;
                    float z2 = position2.Z - position0.Z;

                    Vector3 tangent = new Vector3(
                        (t2 * x1 - t1 * x2) * r,
                        (t2 * y1 - t1 * y2) * r,
                        (t2 * z1 - t1 * z2) * r);

                    Vector3 binormal = new Vector3(
                        (s1 * x2 - s2 * x1) * r,
                        (s1 * y2 - s2 * y1) * r,
                        (s1 * z2 - s2 * z1) * r);

                    vertices[index_vert0].TangentXZ += tangent;
                    vertices[index_vert1].TangentXZ += tangent;
                    vertices[index_vert2].TangentXZ += tangent;

                    vertices[index_vert0].BinormalXZ += binormal;
                    vertices[index_vert1].BinormalXZ += binormal;
                    vertices[index_vert2].BinormalXZ += binormal;
                }
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].TangentXY.Normalize();
                vertices[i].BinormalXY.Normalize();
                vertices[i].TangentXZ.Normalize();
                vertices[i].BinormalXZ.Normalize();
            }
        }
    }
}