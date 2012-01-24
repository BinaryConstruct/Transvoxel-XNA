namespace TransvoxelXna.VolumeData
{
    public class VolumeChunk : VolumeDataBase
    {
        public const int CHUNKBITS = 3;
        public const int CHUNKSIZE = 1 << CHUNKBITS; // = 2^(CHUNKBITS)
        public const int CHUNKMASK = 0x7;

        private readonly sbyte[] values = new sbyte[CHUNKSIZE * CHUNKSIZE * CHUNKSIZE];

        public override sbyte this[int x, int y, int z] 
        {
            get
            {
                return values[Coord2Index(x & CHUNKMASK, y & CHUNKMASK, z & CHUNKMASK)];
            }

            set
            {
                values[Coord2Index(x & CHUNKMASK, y & CHUNKMASK, z & CHUNKMASK)] = value;
            }
        }

        private int Coord2Index(int x, int y, int z)
        {
            return x + y * CHUNKSIZE + z * CHUNKSIZE * CHUNKSIZE;
        }
    }
}
