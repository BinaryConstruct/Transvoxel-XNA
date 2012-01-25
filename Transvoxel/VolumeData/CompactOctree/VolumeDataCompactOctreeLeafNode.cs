using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransvoxelXna.VolumeData.CompactOctree
{
    internal class OctreeLeafNode : OctreeNode
    {
        private VolumeChunk chunk = new VolumeChunk();
  
        public OctreeLeafNode(OctreeChildNode parent, int x, int y, int z, int bitlevel)
            : base(parent, x, y, z, bitlevel)
        { }

        internal override void Set(int x, int y, int z, sbyte val, int bitlevel)
        {
            int equalOffsetNum = EqualOffsetNum(x, y, z, bitlevel);

            if (equalOffsetNum == offsetBitNum)
            {
                //existing leaf - set the value on this node
                chunk[x, y, z] = val;
            }
            else
            {
                //between this node and it's parent, a new node is created
                //on this new node another new leaf node is attached

                //Create the Node at the childIndex of the current node
                int currentChildIndex = parent.GetChildIndex(this);
                OctreeChildNode newc = parent.initChild(currentChildIndex, x, y, z, bitlevel);
                //set the offsetBitNum of the new node to the num of leading equal bits
                newc.offsetBitNum = equalOffsetNum;
                //increase the bitlevel by the offsetbitnum of the created parent
                bitlevel += equalOffsetNum;

                //the current node get's attached to it's newly generated parent node
                int bitIndex = BitHack.BitIndex(xcoord, ycoord, zcoord, bitlevel);
                newc.ReferChild(this, bitIndex);
                offsetBitNum -= (equalOffsetNum + 1);

                //a sibling for this node, or child for the parent node is generated, to hold the new value
                bitIndex = BitHack.BitIndex(x, y, z, bitlevel);
                OctreeLeafNode leaf = newc.initLeaf(bitIndex, x, y, z, bitlevel);
                leaf.offsetBitNum -= 1; //bitIndex adressing consumes 1bit
                leaf.chunk[x, y, z] = val;
            }
        }

        internal override sbyte Get(int x, int y, int z, int bitlevel)
        {
            return chunk[x, y, z];
        }

        internal void setChunkVal(int x, int y, int z, sbyte val)
        {
            chunk[x, y, z] = val;
        }

        public override string ToString(int lz)
        {
            string lzstr = new string(' ', lz);
            string ret = lzstr + base.ToString(lz);
            return ret;
        }
    }
}
