using TransvoxelXna.Helper;

namespace TransvoxelXna
{
    public class Cell
    {
        public byte CaseIndex;
        public readonly int[] Verts;

        public Cell(int size)
        {
            Verts = new int[size];
        }
    }

    public class RegularCache
    {
        private readonly Cell[] _cache;

        public RegularCache()
        {
            const int cacheSize = 2 * Transvoxel.BlockWidth * Transvoxel.BlockWidth;
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
                return _cache[x + y * Transvoxel.BlockWidth + (z & 1) * Transvoxel.BlockWidth * Transvoxel.BlockWidth];
            }
            set
            {
                _cache[x + y * Transvoxel.BlockWidth + (z & 1) * Transvoxel.BlockWidth * Transvoxel.BlockWidth] = value;
            }
        }

        public Cell this[Vector3i v]
        {
            get { return this[v.X, v.Y, v.Z]; }
            set { this[v.X, v.Y, v.Z] = value; }
        }
    }

    public class TransitionCache
    {
        private readonly Cell[] _cache;

        public TransitionCache()
        {
            const int cacheSize = 2 * Transvoxel.BlockWidth * Transvoxel.BlockWidth;
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
                return _cache[x + (y & 1) * Transvoxel.BlockWidth];
            }
            set
            {
                _cache[x + (y & 1) * Transvoxel.BlockWidth] = value;
            }
        }
    }
}