using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Core;

namespace GameEngineTest.Engine.Geometry
{
    public static class TriangleProcessor
    {
        public static void BuildTangentSpaceDataForTriangleList(short[] indices, VertexPositionNormalTextureBump[] vertices)
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
                normal.Normalize();
                vertices[index_vert0].Normal += normal;
                vertices[index_vert1].Normal += normal;
                vertices[index_vert2].Normal += normal;


                Vector2 uv0 = vertices[index_vert0].TextureCoordinate;
                Vector2 uv1 = vertices[index_vert1].TextureCoordinate;
                Vector2 uv2 = vertices[index_vert2].TextureCoordinate;

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
                vertices[i].Tangent.Normalize();
                vertices[i].Binormal.Normalize();
            }
        }

        public static void BuildTangentSpaceDataForTriangleList(short[] indices, Vector3[] positions, Vector2[] uvs, Vector3[] tangents, Vector3[] binormals, Vector3[] normals)
        {
            for (int i = 0; i < positions.Length; i++)
            {
                tangents[i] = new Vector3(0, 0, 0);
                binormals[i] = new Vector3(0, 0, 0);
                normals[i] = new Vector3(0, 0, 0);
            }

            for (int i = 0; i < indices.Length; i += 3)
            {
                int index_vert0 = indices[i];
                int index_vert1 = indices[i + 1];
                int index_vert2 = indices[i + 2];

                Vector3 firstvec = positions[index_vert1] - positions[index_vert0];
                Vector3 secondvec = positions[index_vert0] - positions[index_vert2];
                Vector3 normal = Vector3.Cross(firstvec, secondvec);
                normal.Normalize();
                normals[index_vert0] += normal;
                normals[index_vert1] += normal;
                normals[index_vert2] += normal;

                Vector2 uv0 = uvs[index_vert0];
                Vector2 uv1 = uvs[index_vert1];
                Vector2 uv2 = uvs[index_vert2];

                float s1 = uv1.X - uv0.X;
                float s2 = uv2.X - uv0.X;
                float t1 = uv1.Y - uv0.Y;
                float t2 = uv2.Y - uv0.Y;

                float r = s1 * t2 - s2 * t1;
                if (r != 0.0f)
                {
                    r = 1.0f / r;

                    Vector3 position0 = positions[index_vert0];
                    Vector3 position1 = positions[index_vert1];
                    Vector3 position2 = positions[index_vert2];

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

                    tangents[index_vert0] += tangent;
                    tangents[index_vert1] += tangent;
                    tangents[index_vert2] += tangent;

                    binormals[index_vert0] += binormal;
                    binormals[index_vert1] += binormal;
                    binormals[index_vert2] += binormal;
                }
            }

            for (int i = 0; i < tangents.Length; i++)
            {
                tangents[i].Normalize();
                binormals[i].Normalize();
            }
        }
    }
}