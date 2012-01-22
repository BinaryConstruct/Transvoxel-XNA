using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace VoxelStuff.VolumeData
{
    public class VolumeData : IVolumeData
    {
        VolumeChunk[,,] data;

        public override sbyte this[int x, int y, int z]
        {
            get
            {
                return data[x / VolumeChunk.CHUNKSIZE, y / VolumeChunk.CHUNKSIZE, z / VolumeChunk.CHUNKSIZE][x, y, z];
            }

            set
            {
                data[x / VolumeChunk.CHUNKSIZE, y / VolumeChunk.CHUNKSIZE, z / VolumeChunk.CHUNKSIZE][x, y, z] = value;
            }
        }
    }
}
