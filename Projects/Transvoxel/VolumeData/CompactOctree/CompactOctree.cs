﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transvoxel.Math;

namespace Transvoxel.VolumeData.CompactOctree
{
    public class CompactOctree : IVolumeData
    {
        private readonly OctreeChildNode _head;

        public CompactOctree()
        {
            _head = new OctreeChildNode(null, 0, 0, 0);
        }

        public OctreeNodeBase Head()
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
                return _head.Get(x, y, z);
            }

            set
            {
                _head.Set(x, y, z, value);
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
