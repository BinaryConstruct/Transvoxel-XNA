using System;
using System.Collections.Generic;
using TransvoxelXna.Math;
using TransvoxelXna.VolumeData;

namespace TransvoxelXna.SurfaceExtractor
{
    public class MeshData
    {
        public MeshData()
        {
            Vertices = new List<Vertex>();
            Indices = new List<int>();
        }
        public List<Vertex> Vertices;
        public List<int> Indices;
    }


    /// <summary>
    /// Stores and creates meshes for chunks, also holds volume data
    /// </summary>
    public class TransvoxelManager
    {
        private readonly Dictionary<Vector3i, MeshData> _meshes;

        public IVolumeData VolumeData;

        public TransvoxelManager(IVolumeData volumeData)
        {
            VolumeData = volumeData;
            _meshes = new Dictionary<Vector3i, MeshData>();
        }

        public MeshData RemoveMesh(Vector3i position)
        {
            MeshData mesh;
            if (_meshes.TryGetValue(position, out mesh))
                _meshes.Remove(position);

            return mesh;
        }

        public MeshData GetMesh(Vector3i position) // lod and transition parameter?
        {
            MeshData mesh;
            if (!_meshes.TryGetValue(position, out mesh))
            {
                mesh = GenerateMesh();
                _meshes.Add(position, mesh);
            }

            return mesh;
        }

        private MeshData GenerateMesh()
        {
            // probably need some parameters...lod, transition, regular etc...
            throw new NotImplementedException();
        }

        /*
        // for reference:
        private void generateNegativeXTransitionCells(
            IVolumeData samples,
            Vector3f offset,
            Vector3i min,
            float cellSize,
            byte lodIndex,
            List<Vertex> verts,
            List<int> indices)
        {
            int spacing = 1 << lodIndex; // Spacing between low-res corners.

            Vector3i origin = min + new Vector3i(0, 0, 16);
            Vector3i xAxis = new Vector3i(0, 0, -1);
            Vector3i yAxis = new Vector3i(0, 1, 0);
            Vector3i zAxis = new Vector3i(1, 0, 0);

            TransitionCache cache = new TransitionCache();

            for (int y = 0; y < 16; ++y)
            {
                for (int x = 0; x < 16; ++x)
                {
                    Vector3i p = (origin + xAxis * x + yAxis * y) * spacing;
                    byte directionMask = (byte)((x > 0 ? 1 : 0) | ((y > 0 ? 1 : 0) << 1));
                    //Transvoxel.PolygonizeTransitionCell(offset, p, xAxis, yAxis, zAxis, x, y, cellSize, lodIndex, 0, directionMask, samples, verts, indices, cache);
                }
            }
        }
         * */
    }
}