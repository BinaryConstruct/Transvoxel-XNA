using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransvoxelXna.Math;

namespace TransvoxelXna.VolumeData.CompactOctree
{
    public class CompactOctree : IVolumeData
    {
        private readonly OctreeChildNode _head;

        public CompactOctree()
        {
            _head = new OctreeChildNode(null, 0, 0, 0, sizeof(int) * 8 - VolumeChunk.CHUNKBITS);
        }

        public OctreeNode Head()
        {
            return _head;
        }

        public sbyte this[int x, int y, int z]
        {
            get
            {
                return _head.Get(x, y, z, 0);
            }

            set
            {
                _head.Set(x, y, z, value, 0);
            }
        }

        public sbyte this[Vector3i v]
        {
            get { return this[v.X, v.Y, v.Z]; }
            set { this[v.X, v.Y, v.Z] = value; }
        }

        public override string ToString()
        {
            return _head.ToString(0);
        }
    }

}
