using Transvoxel.Math;
using Transvoxel.VolumeData.CompactOctree;
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
        private readonly ReuseCell[] _cache;

        public RegularCellCache()
        {
            const int cacheSize = /*2*/ VolumeChunk.CHUNKSIZE * VolumeChunk.CHUNKSIZE * VolumeChunk.CHUNKSIZE;
            _cache = new ReuseCell[cacheSize];

            for (int i = 0; i < cacheSize; i++)
            {
                _cache[i] = new ReuseCell(4);
            }
        }

        public ReuseCell this[int x, int y, int z]
        {
            get
            {
                Debug.Assert(x >= 0 && y >= 0 && z >= 0);
                return _cache[(x)/*&1)*/* VolumeChunk.CHUNKSIZE * VolumeChunk.CHUNKSIZE+y*VolumeChunk.CHUNKSIZE+z];
            }
            set
            {
                Debug.Assert(x >= 0 && y >= 0 && z >= 0);
                _cache[(x/* & 1*/) * VolumeChunk.CHUNKSIZE * VolumeChunk.CHUNKSIZE + y * VolumeChunk.CHUNKSIZE + z] = value;
            }
        }

        public ReuseCell this[Vector3i v]
        {
            get { return this[v.X, v.Y, v.Z]; }
            set { this[v.X, v.Y, v.Z] = value; }
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