using Transvoxel.Math;
using System.Diagnostics;

namespace Transvoxel.SurfaceExtractor
{
    internal class ReuseCell
    {
        public readonly int[] Verts;

        public ReuseCell(int size)
        {
            Verts = new int[size];

            for (int i = 0; i < size; i++)
                Verts[i] = -1;
        }
    }
    internal class RegularCellCache
    {
        private readonly ReuseCell[][] _cache;
        private int chunkSize;

        public RegularCellCache(int chunksize)
        {
            this.chunkSize = chunksize;
            _cache = new ReuseCell[2][];

            _cache[0] = new ReuseCell[chunkSize * chunkSize];
            _cache[1] = new ReuseCell[chunkSize * chunkSize];

            for (int i = 0; i < chunkSize * chunkSize; i++)
            {
                _cache[0][i] = new ReuseCell(4);
                _cache[1][i] = new ReuseCell(4);
            }
        }

        public ReuseCell GetReusedIndex(Vector3i pos, byte rDir)
        {
            int rx = rDir & 0x01;
            int rz = (rDir >> 1) & 0x01;
            int ry = (rDir >> 2) & 0x01;

            int dx = pos.X - rx;
            int dy = pos.Y - ry;
            int dz = pos.Z - rz;

            Debug.Assert(dx >= 0 && dy >= 0 && dz >= 0);
            return _cache[dx & 1][dy * chunkSize + dz];
        }


        public ReuseCell this[int x, int y, int z]
        {
            set
            {
                Debug.Assert(x >= 0 && y >= 0 && z >= 0);
                _cache[x & 1][y * chunkSize + z] = value;
            }
        }

        public ReuseCell this[Vector3i v]
        {
            set { this[v.X, v.Y, v.Z] = value; }
        }


        internal void SetReusableIndex(Vector3i pos, byte reuseIndex, ushort p)
        {
            _cache[pos.X & 1][pos.Y * chunkSize + pos.Z].Verts[reuseIndex] = p;
        }
    }

    internal class TransitionCache
    {
        private readonly ReuseCell[] _cache;

        public TransitionCache()
        {
            const int cacheSize = 0;// 2 * TransvoxelExtractor.BlockWidth * TransvoxelExtractor.BlockWidth;
            _cache = new ReuseCell[cacheSize];

            for (int i = 0; i < cacheSize; i++)
            {
                _cache[i] = new ReuseCell(12);
            }
        }

        public ReuseCell this[int x, int y]
        {
            get
            {
                return null;//_cache[x + (y & 1) * TransvoxelExtractor.BlockWidth];
            }
            set
            {
                //_cache[x + (y & 1) * TransvoxelExtractor.BlockWidth] = value;
            }
        }
    }

}