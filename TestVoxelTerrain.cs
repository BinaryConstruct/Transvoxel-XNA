using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using SynapseGaming.LightingSystem.Core;
using VoxelTest;

namespace GameEngineTest.Engine.Geometry
{
    public class TestVoxelTerrain
    {
        public static void GenerateVoxelMesh(out List<VertexPositionNormalTextureBump> Vertices, out List<short> Indices)
        {
            Vertices = new List<VertexPositionNormalTextureBump>();
            Indices = new List<short>();

            Vector3Int32 xyz = Vector3Int32.Zero;
            Vector3 offset = Vector3.Zero;
            Random r = new Random();
            IVolumeData samples = new VolumeData(new Vector3Int32(16,16,16), (Vector3Int32)offset);
            byte lodIndex = 0;
            float cellSize = 15F;
            IList<Vertex> verts = new List<Vertex>();
            IList<int> indices = new List<int>();
            RegularCache cache = new RegularCache();

            // create some volume data
            for (int x = 0; x < 16; x++)
            {
                for (int z = 0; z < 16; z++)
                {
                    int height = (int)((Math.Sin(x * 0.5f) * Math.Sin(z * 0.5f)) * 3) + 4;


                    for (int y = 0; y < 16; y++)
                    {
                        var p = new Vector3(x, y, z);
                        var o = new Vector3(8, 8, 8);
                        
                        if (y < height || Vector3.DistanceSquared(p, o) <= 24)
                            samples[x, y, z] = 1;  
                        else
                        {
                            samples[x, y, z] = -1;
                        }

                        //samples[x, y, z] = (sbyte)(r.Next(2) - 1);
                    }
                }
            }


            for (int z = 0; z < 16; z++)
            {
                for (int y = 0; y < 16; y++)
                {
                    for (int x = 0; x < 16; x++)
                    {
                        var position = new Vector3Int32(x, y, z);
                        int actual = Transvoxel.PolygonizeRegularCell(position, offset, xyz, samples, lodIndex, cellSize, ref verts, ref indices, ref cache);
                    }
                }
            }

            for (int i = 0; i < verts.Count; i++)
            {
                Vertices.Add(new VertexPositionNormalTextureBump { Position = verts[i].Primary, TextureCoordinate = new Vector2(verts[i].Primary.X, verts[i].Primary.Z) });
            }
            for (int i = 0; i < indices.Count; i++)
            {
                Indices.Add((short)indices[i]);
            }
        }
    }
}