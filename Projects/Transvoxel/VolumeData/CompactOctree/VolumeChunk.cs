using Transvoxel.Math;

namespace Transvoxel.VolumeData.CompactOctree
{
    
    /*
     * VolumeChunk represents the smallest unit in the octree
     * it holds a sbyte array with all voxel densities of a CHUNKSIZE^3 cube
     * 
     * VolumeChunk is also the smallest unit for triangulation at level of detail 0
     * 
     */
    public class VolumeChunk : IVolumeData
    {
        // the used bits of a coordinate to adress a voxel in the chunk
        public const int CHUNKBITS = 3;

        // = 2^(CHUNKBITS)
        public const int CHUNKSIZE = 1 << CHUNKBITS; 

        //For a 3 ChunkBits the Mask would be 0x7
        public static readonly int CHUNKMASK = (int)BitHack.Mask(sizeof(int)*8-CHUNKBITS,sizeof(int)*8-1);

        //signed bit density values
        private readonly sbyte[] values = new sbyte[CHUNKSIZE * CHUNKSIZE * CHUNKSIZE];

        public sbyte this[int x, int y, int z] 
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

        public bool ContainsOnlyZero()
        { 
            int i=0;
            foreach(sbyte s in values)
            {
                i+=s;
            }
            
            return i==0?true:false;
        }

        public sbyte this[Vector3i v]
        {
            get { return this[v.X, v.Y, v.Z]; }
            set { this[v.X, v.Y, v.Z] = value; }
        }

        //calculates 1 dimensional index from 3 dimensional coords
        private int Coord2Index(int x, int y, int z)
        {
            return x + y * CHUNKSIZE + z * CHUNKSIZE * CHUNKSIZE;
        }
    }
}
