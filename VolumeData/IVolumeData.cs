using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using VoxelStuff.helper;

namespace VoxelStuff.VolumeData
{
    public abstract class IVolumeData
    {
        public abstract sbyte this[int x, int y, int z] { get; set; }

        public sbyte this[Vector3i v]
        {
            get { return this[v.X, v.Y, v.Z]; }
            set { this[v.X, v.Y, v.Z] = value; }
        }
    }
}
