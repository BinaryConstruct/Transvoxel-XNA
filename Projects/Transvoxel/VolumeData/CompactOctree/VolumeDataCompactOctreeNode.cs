using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transvoxel.Math;

namespace Transvoxel.VolumeData.CompactOctree
{
    public abstract class OctreeNode
    {
        internal int offsetBitNum = 0;  //equal bits for all subnodes
        internal int level = 0;         //octree depth (before the offset bits)
        internal int xcoord = 0;
        internal int ycoord = 0;
        internal int zcoord = 0;
        internal OctreeChildNode parent;

        internal OctreeNode(OctreeChildNode parent, int x, int y, int z)
        {
            this.parent = parent;
            xcoord = x;
            ycoord = y;
            zcoord = z;
            offsetBitNum = 0;
            level = 0;
        }

        public abstract OctreeNode GetNode(int x, int y, int z);
        internal abstract sbyte Get(int x, int y, int z);
        internal abstract void Set(int x, int y, int z, sbyte val);
        internal abstract bool HasChilds();

        public virtual string ToString(int lz)
        {
            string lzstr = new string(' ', lz);
            return lzstr + "< " + this.GetType().ToString() + " > offsetBitNum:" + offsetBitNum+" lvl:"+GetLevel()+" lod:"+GetLevelOfDetail();
        }

        private int GetLevel()
        {
            return level+offsetBitNum;//((parent==null?0:parent.GetLevel()+1)+offsetBitNum);
        }

        // from 1 to infinity
        public int GetLevelOfDetail()
        {
            return 30 - GetLevel();
        }

        public Vector3i GetPos()
        {
            int mask = (int)BitHack.Mask(0, level + offsetBitNum);
            int x = xcoord & mask;
            int y = ycoord & mask;
            int z = zcoord & mask;
            return new Vector3i(x,y,z);
        }

        public int Size()
        { 
            return GetLevelOfDetail()*VolumeChunk.CHUNKSIZE;
        }

        public Vector3i GetCenter()
        { 
            int szh = Size()/2;
            return GetPos() + new Vector3i(szh, szh, szh);
        }

        // calculates the num of equal bits between bitlevel and offsetBitNum
        // Example:
        //      x: 00000000000
        // xcoord: 00000111001
        // from left to right the number of equal bits is 5
        // this is done for x,y and z coordinate, minimum of those 3 is returned
        
        internal int EqualOffsetNum(int x, int y, int z)
        {
            int equalX = BitHack.cmpBit(xcoord, x, level, offsetBitNum);
            int equalY = BitHack.cmpBit(ycoord, y, level, offsetBitNum);
            int equalZ = BitHack.cmpBit(zcoord, z, level, offsetBitNum);

            return BitHack.min(equalX, equalY, equalZ);
        }
    }
}
