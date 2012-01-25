using TransvoxelXna.Helper;
namespace TransvoxelXna.VolumeData
{
    
    /*
     * VolumeChunk represents the smallest unit in the octree
     * it holds a sbyte array with all voxel densities of a CHUNKSIZE^3 cube
     * the density of a voxel can be adressed 
     */
    public class VolumeChunk : VolumeDataBase
    {
        // the used bits of a coordinate to adress a voxel in the chunk
        public static readonly int CHUNKBITS = 3;

        // = 2^(CHUNKBITS)
        public static readonly int CHUNKSIZE = 1 << CHUNKBITS; 

        //For a 3 ChunkBits the Mask would be 0x7
        public static readonly int CHUNKMASK = (int)MathHelper.Mask(sizeof(int)*8-CHUNKBITS,sizeof(int)*8-1);

        //signed bit density values
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

        //calculates 1 dimensional index from 3 dimensional
        private int Coord2Index(int x, int y, int z)
        {
            return x + y * CHUNKSIZE + z * CHUNKSIZE * CHUNKSIZE;
        }
    }
}
