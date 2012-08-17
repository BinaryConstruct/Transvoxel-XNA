﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transvoxel.VolumeData.CompactOctree;
using Transvoxel.Geometry;
using Transvoxel.VolumeData;
using Transvoxel.Math;
using Transvoxel.Lengyel;
using System.Diagnostics;

namespace Transvoxel.SurfaceExtractor
{
    public interface ISurfaceExtractor
    {
        Mesh GenLodRegion(Vector3i min, int size,int lod);
        Mesh GenLodCell(OctreeNode<sbyte> n);
    }

    public class TransvoxelExtractor : ISurfaceExtractor
    {
        public bool UseCache { get; set; }
        IVolumeData volume;
        RegularCellCache cache;

        public TransvoxelExtractor(IVolumeData data)
        {
            volume = data;
            cache = new RegularCellCache(volume.ChunkSize * 10);
            UseCache = true;
        }

        public Mesh GenLodRegion(Vector3i min, int size, int lod)
        {
            Mesh mesh = new Mesh();
            for (int x = 0; x < size ; x++)
            {
                for (int y = 0; y < size; y ++)
                {
                    for (int z = 0; z < size; z ++)
                    {
                        Vector3i position = new Vector3i(x, y, z);
                        PolygonizeCell(min, position, ref mesh, lod);
                    }
                }
            }
            return mesh;
        }

        public Mesh GenLodCell(OctreeNode<sbyte> node)
        {
            Mesh mesh = new Mesh();
            int lod = 1 << (node.GetLod() - volume.ChunkBits);

            for (int x = 0; x < volume.ChunkSize; x++)
            {
                for (int y = 0; y < volume.ChunkSize; y++)
                {
                    for (int z = 0; z < volume.ChunkSize; z++)
                    {
                        Vector3i position; //new Vector3i(x, y, z);
                        position.X = x;
                        position.Y = y;
                        position.Z = z;
                        PolygonizeCell(node.GetPos(), position, ref mesh, lod);
                    }
                }
            }

            return mesh;
        }

        internal void PolygonizeCell(Vector3i offsetPos, Vector3i pos, ref Mesh mesh, int lod)
        {
            Debug.Assert(lod >= 1, "Level of Detail must be greater than 1");
            offsetPos += pos * lod;

            byte directionMask = (byte)((pos.X > 0 ? 1 : 0) | ((pos.Z > 0 ? 1 : 0) << 1) | ((pos.Y > 0 ? 1 : 0) << 2));

            sbyte[] density = new sbyte[8];

            for (int i = 0; i < density.Length; i++)
            {
                density[i] = volume[offsetPos + Tables.CornerIndex[i] * lod];
            }

            byte caseCode = getCaseCode(density);
            if ((caseCode ^ ((density[7] >> 7) & 0xFF)) == 0) //for this cases there is no triangulation
                return;

            Vector3f[] cornerNormals = new Vector3f[8];
            for (int i = 0; i < 8; i++)
            {
                var p = offsetPos + Tables.CornerIndex[i] * lod;
                float nx = (volume[p + Vector3i.UnitX] - volume[p - Vector3i.UnitX]) * 0.5f;
                float ny = (volume[p + Vector3i.UnitY] - volume[p - Vector3i.UnitY]) * 0.5f;
                float nz = (volume[p + Vector3i.UnitZ] - volume[p - Vector3i.UnitZ]) * 0.5f;
                //cornerNormals[i] = new Vector3f(nx, ny, nz);

                cornerNormals[i].X = nx;
                cornerNormals[i].Y = ny;
                cornerNormals[i].Z = nz;
                cornerNormals[i].Normalize();
            }

            byte regularCellClass = Tables.RegularCellClass[caseCode];
            ushort[] vertexLocations = Tables.RegularVertexData[caseCode];

            Tables.RegularCell c = Tables.RegularCellData[regularCellClass];
            long vertexCount = c.GetVertexCount();
            long triangleCount = c.GetTriangleCount();
            byte[] indexOffset = c.Indizes(); //index offsets for current cell
            ushort[] mappedIndizes = new ushort[indexOffset.Length]; //array with real indizes for current cell

            for (int i = 0; i < vertexCount; i++)
            {
                byte edge = (byte)(vertexLocations[i] >> 8);
                byte reuseIndex = (byte)(edge & 0xF); //Vertex id which should be created or reused 1,2 or 3
                byte rDir = (byte)(edge >> 4); //the direction to go to reach a previous cell for reusing 

                byte v1 = (byte)((vertexLocations[i]) & 0x0F); //Second Corner Index
                byte v0 = (byte)((vertexLocations[i] >> 4) & 0x0F); //First Corner Index

                sbyte d0 = density[v0];
                sbyte d1 = density[v1];

                //Vector3f n0 = cornerNormals[v0];
                //Vector3f n1 = cornerNormals[v1];

                Debug.Assert(v1 > v0);

                int t = (d1 << 8) / (d1 - d0);
                int u = 0x0100 - t;
                float t0 = t / 256f;
                float t1 = u / 256f;

                int index = -1;

                if (UseCache && v1 != 7 && (rDir & directionMask) == rDir)
                {
                    Debug.Assert(reuseIndex != 0);
                    ReuseCell cell = cache.GetReusedIndex(pos, rDir);
                    index = cell.Verts[reuseIndex];
                }

                if (index == -1)
                {
                    Vector3f normal = cornerNormals[v0] * t0 + cornerNormals[v1] * t1;
                    GenerateVertex(ref offsetPos, ref pos, mesh, lod, t, ref v0, ref v1, ref d0, ref d1, normal);
                    index = mesh.LatestAddedVertIndex();
                }

                if ((rDir & 8) != 0)
                {
                    cache.SetReusableIndex(pos, reuseIndex, mesh.LatestAddedVertIndex());
                }

                mappedIndizes[i] = (ushort)index;
            }

            for (int t = 0; t < triangleCount; t++)
            {
                for (int i = 0; i < 3; i++)
                {
                    mesh.AddIndex(mappedIndizes[c.Indizes()[t * 3 + i]]);
                }
            }
        }

        private void GenerateVertex(ref Vector3i offsetPos, ref Vector3i pos, Mesh mesh, int lod, long t, ref byte v0, ref byte v1, ref sbyte d0, ref sbyte d1, Vector3f normal)
        {
            Vector3i iP0 = (offsetPos + Tables.CornerIndex[v0] * lod);
            Vector3f P0;// = new Vector3f(iP0.X, iP0.Y, iP0.Z);
            P0.X = iP0.X;
            P0.Y = iP0.Y;
            P0.Z = iP0.Z;

            Vector3i iP1 = (offsetPos + Tables.CornerIndex[v1] * lod);
            Vector3f P1;// = new Vector3f(iP1.X, iP1.Y, iP1.Z);
            P1.X = iP1.X;
            P1.Y = iP1.Y;
            P1.Z = iP1.Z;

            //EliminateLodPositionShift(lod, ref d0, ref d1, ref t, ref iP0, ref P0, ref iP1, ref P1);


            Vector3f Q = InterpolateVoxelVector(t, P0, P1);

            mesh.AddVertex(Q, normal);
        }

        private void EliminateLodPositionShift(int lod, ref sbyte d0, ref sbyte d1, ref long t, ref Vector3i iP0, ref Vector3f P0, ref Vector3i iP1, ref Vector3f P1)
        {


            for (int k = 0; k < lod - 1; k++)
            {
                Vector3f vm = (P0 + P1) / 2.0f;
                Vector3i pm = (iP0 + iP1) / 2;
                sbyte sm = volume[pm];

                if ((d0 & 0x8F) != (d1 & 0x8F))
                {
                    P1 = vm;
                    iP1 = pm;
                    d1 = sm;
                }
                else
                {
                    P0 = vm;
                    iP0 = pm;
                    d0 = sm;
                }
            }

            if (d1 == d0) // ?????????????
                return;
            t = (d1 << 8) / (d1 - d0); // recalc
        }

        /*private static ReuseCell getReuseCell(ReuseCell[, ,] cells, int lod, byte rDir, Vector3i pos)
        {
            int rx = rDir & 0x01;
            int rz = (rDir >> 1) & 0x01;
            int ry = (rDir >> 2) & 0x01;

            int dx = pos.X / lod - rx;
            int dy = pos.Y / lod - ry;
            int dz = pos.Z / lod - rz;

            ReuseCell ccc = cells[dx, dy, dz];
            return ccc;
        }*/

        internal static Vector3f InterpolateVoxelVector(long t, Vector3f P0, Vector3f P1)
        {
            long u = 0x0100 - t; //256 - t
            float s = 1.0f / 256.0f;
            Vector3f Q = P0 * t + P1 * u; //Density Interpolation
            Q *= s; // shift to shader ! 
            return Q;
        }

        /* internal void mapIndizes2Vertice(int vertexNr, ushort index, ushort[] mappedIndizes, byte[] indexOffset)
         {
             for (int j = 0; j < mappedIndizes.Length; j++)
             {
                 if (vertexNr == indexOffset[j])
                 {
                     mappedIndizes[j] = index;
                 }
             }
         }*/

        private static byte getCaseCode(sbyte[] density)
        {
            byte code = 0;
            byte konj = 0x01;
            for (int i = 0; i < density.Length; i++)
            {
                code |= (byte)((density[i] >> (density.Length - 1 - i)) & konj);
                konj <<= 1;
            }

            return code;
        }
    }
}
