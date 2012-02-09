using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransvoxelXna.VolumeData.CompactOctree
{
    public abstract class OctreeNode
    {
        internal int offsetBitNum = 0;
        internal int xcoord = 0;
        internal int ycoord = 0;
        internal int zcoord = 0;
        internal OctreeChildNode parent;
        internal int level = 0;


        internal OctreeNode(OctreeChildNode parent, int x, int y, int z, int bitlevel)
        {
            this.parent = parent;
            xcoord = x;
            ycoord = y;
            zcoord = z;
            offsetBitNum = sizeof(int) * 8 - bitlevel - VolumeChunk.CHUNKBITS;
        }

        internal abstract sbyte Get(int x, int y, int z, int bitlevel);
        internal abstract void Set(int x, int y, int z, sbyte val, int bitlevel);
        internal abstract bool HasChilds();

        public virtual string ToString(int lz)
        {
            string lzstr = new string(' ', lz);
            return lzstr + "< " + this.GetType().ToString() + " > offsetBitNum:" + offsetBitNum+" lvl:"+GetLevel();
        }

        private int GetLevel()
        {
            return ((parent==null?0:parent.GetLevel()+1)+offsetBitNum);
        }

        // from 1 to infinity
        public int GetLevelOfDetail()
        {
            return sizeof(int) * 8 - VolumeChunk.CHUNKBITS - GetLevel()+1;
        }

        // calculates the num of equal bits between bitlevel and offsetBitNum
        // Example:
        //      x: 00000000000
        // xcoord: 00000111001
        // from left to right the number of equal bits is 5
        // this is done for x,y and z coordinate, minimum of those 3 is returned
        
        internal int EqualOffsetNum(int x, int y, int z, int bitlevel)
        {
            int equalX = BitHack.cmpBit(xcoord, x, bitlevel, offsetBitNum);
            int equalY = BitHack.cmpBit(ycoord, y, bitlevel, offsetBitNum);
            int equalZ = BitHack.cmpBit(zcoord, z, bitlevel, offsetBitNum);

            return BitHack.min(equalX, equalY, equalZ);
        }
    }
}
