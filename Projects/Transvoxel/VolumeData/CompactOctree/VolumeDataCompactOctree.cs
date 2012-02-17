using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transvoxel.Math;

namespace Transvoxel.VolumeData.CompactOctree
{
    public class CompactOctree : IVolumeData
    {
        private int chunkbits = 4;
        private readonly OctreeIterator<sbyte> iterator;
        private readonly OctreeNode<sbyte> _head;

        public CompactOctree()
        {
            _head = new OctreeNode<sbyte>(null, 0, 0, 0);
            iterator = new OctreeIterator<sbyte>(_head);
        }

        public OctreeNode<sbyte> Head()
        {
            return _head;
        }

        public int ChunkSize
        {
            get { return 1<<chunkbits; }
            set { chunkbits = (int)System.Math.Log(value, 2); }
        }

        public int ChunkBits
        {
            get { return chunkbits; }
            set { chunkbits = value; }
        }

        // Very expensive method, has to traverse the complete octree
        public sbyte this[int x, int y, int z]
        {
            get
            {
                return _head.GetValue((short)x, (short)y, (short)z, 32);
            }

            set
            {
                _head.Set((short)x, (short)y, (short)z, value, 32);
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
