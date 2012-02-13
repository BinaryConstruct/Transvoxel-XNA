using Microsoft.Xna.Framework;
using Transvoxel.Geometry;
using Transvoxel.Math;

namespace TransvoxelXnaStudio.TransvoxelHelpers
{
    public static class Converters
    {
        public static Vector3 Vector3fToVector3(Vector3f v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }
        public static Vector3f Vector3ToVector3f(Vector3 v)
        {
            return new Vector3f(v.X, v.Y, v.Z);
        }
        public static Vector3 Vector3iToVector3(Vector3i v)
        {
            return new Vector3(v.X, v.Y, v.Z);
        }
        public static Vector3i Vector3ToVector3i(Vector3 v)
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

                 vertices[vert] = new VertexPositionTextureNormalColor(Vector3fToVector3(m.Vertices[vert]), uv, color.ToVector4());
             }

             CalculateNormals(ref vertices, ref i);

            return vertices;
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