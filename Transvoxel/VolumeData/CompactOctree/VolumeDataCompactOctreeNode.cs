using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransvoxelXna.VolumeData.CompactOctree
{
    internal abstract class OctreeNode
    {
        internal int offsetBitNum = 0;
        internal int xcoord = 0;
        internal int ycoord = 0;
        internal int zcoord = 0;
        internal OctreeChildNode parent;

        public OctreeNode(OctreeChildNode parent, int x, int y, int z, int bitlevel)
        {
            this.parent = parent;
            xcoord = x;
            ycoord = y;
            zcoord = z;
            offsetBitNum = sizeof(int) * 8 - bitlevel - VolumeChunk.CHUNKBITS;
        }

        public abstract sbyte Get(int x, int y, int z, int bitlevel);
        public abstract void Set(int x, int y, int z, sbyte val, int bitlevel);

        public virtual string ToString(int lz)
        {
            string lzstr = new string(' ', lz);
            return lzstr + "< " + this.GetType().ToString() + " > offsetBitNum:" + offsetBitNum;
        }
    }
}
