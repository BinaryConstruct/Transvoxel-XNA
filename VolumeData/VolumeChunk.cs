using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoxelStuff.helper;

namespace VoxelStuff.VolumeData
{
    public class VolumeChunk : IVolumeData
    {
        public const int CHUNKSIZE = 16;
        private readonly sbyte[] values = new sbyte[CHUNKSIZE * CHUNKSIZE * CHUNKSIZE];

        public override sbyte this[int x, int y, int z] 
        {
            get
            {
                return values[Coord2Index(x,y,z)];
            }

            set
            {
                values[Coord2Index(x, y, z)] = value;
            }
        }

        private int Coord2Index(int x, int y, int z)
        {
            return x + y * CHUNKSIZE + z * CHUNKSIZE * CHUNKSIZE;
        }
    }
}
