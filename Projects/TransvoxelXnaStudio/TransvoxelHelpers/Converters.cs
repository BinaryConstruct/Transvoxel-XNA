using Microsoft.Xna.Framework;
using Transvoxel.Geometry;
using Transvoxel.Math;

namespace TransvoxelXnaStudio.TransvoxelHelpers
{
    public static class Converters
    {
        public static Vector3 ToVector3(this Vector3f v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }
        public static Vector3f ToVector3F(this Vector3 v)
        {
            return new Vector3f(v.X, v.Y, v.Z);
        }
        public static Vector3 ToVector3(this Vector3i v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }
        public static Vector3i ToVector3I(this Vector3 v)
        {
            return new Vector3i((int)v.X, (int)v.Y, (int)v.Z);
        }

        public static VertexPositionTextureNormalColor[] ConvertMeshToXna(Mesh m, Color color)
        {
            var i = m.GetIndices();
            VertexPositionTextureNormalColor[] vertices = new VertexPositionTextureNormalColor[m.Vertices.Count];

            // minimize garbage by accessing list
            for (int vert = 0; vert < m.Vertices.Count; vert++)
            {
                // project texture coord using x/y
                var uv = new Vector2(m.Vertices[vert].X, m.Vertices[vert].Y);

                vertices[vert] = new VertexPositionTextureNormalColor(m.Vertices[vert].ToVector3(), uv, color.ToVector4());
            }

            BuildTangentSpaceDataForTriangleList(ref vertices, ref i);

            return vertices;
        }

        public static void BuildTangentSpaceDataForTriangleList(ref VertexPositionTextureNormalColor[] vertices, ref ushort[] indices)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Normal = new Vector3(0, 0, 0);
                vertices[i].Tangent = new Vector3(0, 0, 0);
                vertices[i].Binormal = new Vector3(0, 0, 0);
            }

            for (int i = 0; i < indices.Length; i += 3)
            {
                int index_vert0 = indices[i];
                int index_vert1 = indices[i + 1];
                int index_vert2 = indices[i + 2];

                Vector3 firstvec = vertices[index_vert1].Position - vertices[index_vert0].Position;
                Vector3 secondvec = vertices[index_vert0].Position - vertices[index_vert2].Position;
                Vector3 normal = Vector3.Cross(firstvec, secondvec);
                vertices[index_vert0].Normal += normal;
                vertices[index_vert1].Normal += normal;
                vertices[index_vert2].Normal += normal;


                Vector2 uv0 = vertices[index_vert0].TextureCoordinate1;
                Vector2 uv1 = vertices[index_vert1].TextureCoordinate1;
                Vector2 uv2 = vertices[index_vert2].TextureCoordinate1;

                float s1 = uv1.X - uv0.X;
                float s2 = uv2.X - uv0.X;
                float t1 = uv1.Y - uv0.Y;
                float t2 = uv2.Y - uv0.Y;

                float r = s1 * t2 - s2 * t1;
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

                    vertices[index_vert0].Tangent += tangent;
                    vertices[index_vert1].Tangent += tangent;
                    vertices[index_vert2].Tangent += tangent;

                    vertices[index_vert0].Binormal += binormal;
                    vertices[index_vert1].Binormal += binormal;
                    vertices[index_vert2].Binormal += binormal;
                }
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Normal.Normalize();
                vertices[i].Tangent.Normalize();
                vertices[i].Binormal.Normalize();
            }
        }

        private static void CalculateNormals(ref VertexPositionTextureNormalColor[] vertices, ref ushort[] indices)
        {
            //for (int i = 0; i < vertices.Length; i++)
            //    vertices[i].Normal = new Vector3(0, 0, 0);

            for (int i = 0; i < indices.Length / 3; i++)
            {
                Vector3 firstvec = vertices[indices[i * 3 + 1]].Position - vertices[indices[i * 3]].Position;
                Vector3 secondvec = vertices[indices[i * 3]].Position - vertices[indices[i * 3 + 2]].Position;
                Vector3 normal = Vector3.Cross(firstvec, secondvec);
                normal.Normalize();
                vertices[indices[i * 3]].Normal += normal;
                vertices[indices[i * 3 + 1]].Normal += normal;
                vertices[indices[i * 3 + 2]].Normal += normal;

                // binormal/tangent
                Vector3 v1 = vertices[indices[i * 3]].Position;
                Vector3 v2 = vertices[indices[i * 3 + 1]].Position;
                Vector3 v3 = vertices[indices[i * 3 + 2]].Position;

                Vector2 w1 = vertices[indices[i * 3]].TextureCoordinate1;
                Vector2 w2 = vertices[indices[i * 3 + 1]].TextureCoordinate1;
                Vector2 w3 = vertices[indices[i * 3 + 2]].TextureCoordinate1;

                float x1 = v2.X - v1.X;
                float x2 = v3.X - v1.X;
                float y1 = v2.Y - v1.Y;
                float y2 = v3.Y - v1.Y;
                float z1 = v2.Z - v1.Z;
                float z2 = v3.Z - v1.Z;

                float s1 = w2.X - w1.X;
                float s2 = w3.X - w1.X;
                float t1 = w2.Y - w1.Y;
                float t2 = w3.Y - w1.Y;

                float r = 1.0f / (s1 * t2 - s2 * t1);
                Vector3 sdir = new Vector3((t2 * x1 - t1 * x2) * r, (t2 * y1 - t1 * y2) * r, (t2 * z1 - t1 * z2) * r);
                Vector3 tdir = new Vector3((s1 * x2 - s2 * x1) * r, (s1 * y2 - s2 * y1) * r, (s1 * z2 - s2 * z1) * r);

                // Gram-Schmidt orthogonalize  
                Vector3 tangent = sdir - normal * Vector3.Dot(normal, sdir);
                tangent.Normalize();
                vertices[indices[i * 3]].Tangent += tangent;
                vertices[indices[i * 3 + 1]].Tangent += tangent;
                vertices[indices[i * 3 + 2]].Tangent += tangent;

                // Calculate handedness (here maybe you need to switch >= with <= depend on the geometry winding order)  
                float tangentdir = (Vector3.Dot(Vector3.Cross(normal, sdir), tdir) <= 0.0f) ? 1.0f : -1.0f;
                Vector3 binormal = Vector3.Cross(normal, tangent) * tangentdir;
                vertices[indices[i * 3]].Binormal += binormal;
                vertices[indices[i * 3 + 1]].Binormal += binormal;
                vertices[indices[i * 3 + 2]].Binormal += binormal;
            }

            for (int i = 0; i < vertices.Length; i++)
            {
                vertices[i].Tangent.Normalize();
                vertices[i].Normal.Normalize();
                vertices[i].Binormal.Normalize();
            }
        }
    }


}