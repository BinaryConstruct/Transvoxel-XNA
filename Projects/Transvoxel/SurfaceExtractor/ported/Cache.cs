using Transvoxel.Math;

namespace Transvoxel.SurfaceExtractor
{
    internal class Cell
    {
        public byte CaseIndex;
        public readonly int[] Verts;

        public Cell(int size)
        {
            Verts = new int[size];
        }
    }
    internal class RegularCache
    {
        private readonly Cell[] _cache;

        public RegularCache()
        {
            const int cacheSize = 0;//2 * TransvoxelExtractor.BlockWidth * TransvoxelExtractor.BlockWidth;
            _cache = new Cell[cacheSize];

            for (int i = 0; i < cacheSize; i++)
            {
                _cache[i] = new Cell(4);
            }
        }

        public Cell this[int x, int y, int z]
        {
            get
            {
                return null;//_cache[x + y * TransvoxelExtractor.BlockWidth + (z & 1) * TransvoxelExtractor.BlockWidth * TransvoxelExtractor.BlockWidth];
            }
            set
            {
                //_cache[x + y * TransvoxelExtractor.BlockWidth + (z & 1) * TransvoxelExtractor.BlockWidth * TransvoxelExtractor.BlockWidth] = value;
            }
        }

        public Cell this[Vector3i v]
        {
            get { return this[v.X, v.Y, v.Z]; }
            set { this[v.X, v.Y, v.Z] = value; }
        }
    }

    internal class TransitionCache
    {
        private readonly Cell[] _cache;

        public TransitionCache()
        {
            const int cacheSize = 0;// 2 * TransvoxelExtractor.BlockWidth * TransvoxelExtractor.BlockWidth;
            _cache = new Cell[cacheSize];

            for (int i = 0; i < cacheSize; i++)
            {
                _cache[i] = new Cell(12);
            }
        }

        public Cell this[int x, int y]
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