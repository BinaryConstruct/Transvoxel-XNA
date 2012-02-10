using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Transvoxel.VolumeData.CompactOctree
{
    internal class OctreeLeafNode : OctreeNode
    {
        private VolumeChunk chunk = new VolumeChunk();

        public OctreeLeafNode(OctreeChildNode parent, int x, int y, int z)
            : base(parent, x, y, z)
        { }

        internal override void Set(int x, int y, int z, sbyte val)
        {
            //Console.WriteLine("SetLeaf "+level+" "+offsetBitNum);
            int equalOffsetNum = EqualOffsetNum(x, y, z, level);

            if (equalOffsetNum == offsetBitNum)
            {
                //existing leaf - set the value on this node
                chunk[x, y, z] = val;
            }
            else
            {
                //Create Node
                OctreeChildNode newc = parent.initChild(parent.GetChildIndex(this), x, y, z);
                newc.offsetBitNum = equalOffsetNum;
                newc.level = parent.level+parent.offsetBitNum+1;

                //Add this as child to the new Node
                int bitIndex = BitHack.BitIndex(xcoord, ycoord, zcoord, newc.level + newc.offsetBitNum);
                newc.ReferChild(this, bitIndex);
                offsetBitNum -= (equalOffsetNum + 1);
                level = newc.level + newc.offsetBitNum + 1;

                bitIndex = BitHack.BitIndex(x, y, z, newc.level + newc.offsetBitNum);
                OctreeLeafNode leaf = newc.initLeaf(bitIndex, x, y, z);
                leaf.level = level;
                leaf.offsetBitNum = offsetBitNum;
                leaf.setChunkVal(x, y, z, val);
                leaf.chunk[x, y, z] = val;

                Debug.Assert(level + offsetBitNum <= 29);
                Debug.Assert(newc.level + newc.offsetBitNum <= 29);
                Debug.Assert(leaf.level + offsetBitNum <= 29);
            }
        }

        internal override sbyte Get(int x, int y, int z)
        {
            return chunk[x, y, z];
        }

        internal void setChunkVal(int x, int y, int z, sbyte val)
        {
            chunk[x, y, z] = val;
        }

        internal override bool HasChilds()
        {
            return false;
        }

        public override string ToString(int lz)
        {
            string lzstr = new string(' ', lz);
            string ret = lzstr + base.ToString(lz);
            return ret;
        }
    }
}
