using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transvoxel.Math;

namespace Transvoxel.VolumeData.CompactOctree
{
    public class CompactOctree : IVolumeData
    {
        private readonly OctreeNode<VolumeChunk> _head;

        public CompactOctree()
        {
            _head = new OctreeNode<VolumeChunk>(null, 0, 0, 0);
        }

        public OctreeNode<VolumeChunk> Head()
        {
            return _head;
        }

        public int ChunkSize
        {
            get { return VolumeChunk.CHUNKSIZE; }
        }

        // Very expensive method, has to traverse the complete octree
        public sbyte this[int x, int y, int z]
        {
            get
            {
                OctreeNode<VolumeChunk> n = _head.Get(x, y, z);
                if (n == null || n.value == null)
                    return (sbyte)0;
                return n.value[x,y,z];
            }

            set
            {
                _head.Set(x, y, z)[x, y, z] = value;
            }
        }

        public sbyte this[Vector3i v]
        {
            get { return this[v.X, v.Y, v.Z]; }
            set { this[v.X, v.Y, v.Z] = value; }
        }       

        public override string ToString()
        {
            return _head.ToStringA(0);
        }

        
    }

}
