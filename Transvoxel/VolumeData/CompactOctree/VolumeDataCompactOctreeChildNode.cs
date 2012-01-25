using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransvoxelXna.VolumeData.CompactOctree
{
    internal class OctreeChildNode : OctreeNode
    {
        private OctreeNode[] nodes = new OctreeNode[8]; //index: x+y*2+z*4 | x,y,z elementof {0,1}

        public OctreeChildNode(OctreeChildNode parent, int x, int y, int z, int bitlevel)
            : base(parent, x, y, z, bitlevel)
        { }

        public OctreeChildNode initChild(int place, int x, int y, int z, int bitlevel)
        {
            OctreeChildNode newChild = new OctreeChildNode(this, x, y, z, bitlevel);
            nodes[place] = newChild;
            return newChild;
        }

        public OctreeLeafNode initLeaf(int place, int x, int y, int z, int bitlevel)
        {
            OctreeLeafNode leaf = new OctreeLeafNode(this, x, y, z, bitlevel);
            nodes[place] = leaf;
            return leaf;
        }

        public int GetChildIndex(OctreeNode n)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                if (n == nodes[i])
                    return i;
            }

            //Throws index out of bounds, because GetChildIndex always adresses an array
            return -1;
        }

        internal void ReferChild(OctreeNode n, int i)
        {
            nodes[i] = n;
            n.parent = this;
        }

        internal override sbyte Get(int x, int y, int z, int bitlevel)
        {
            int equalOffsetNum = EqualOffsetNum(x, y, z, bitlevel);

            if (equalOffsetNum == offsetBitNum)
            {
                bitlevel += offsetBitNum;
            }
            else
            {
                return 0;
            }

            int bitIndex = BitHack.BitIndex(x, y, z, bitlevel);

            if (nodes[bitIndex] == null)
            {
                return 0;
            }

            return nodes[bitIndex].Get(x, y, z, bitlevel + 1);
        }

        internal override void Set(int x, int y, int z, sbyte val, int bitlevel)
        {
            int equalOffsetNum = EqualOffsetNum(x, y, z, bitlevel);

            if (equalOffsetNum == offsetBitNum)
            {
                bitlevel += offsetBitNum;

                int bitIndex = BitHack.BitIndex(x, y, z, bitlevel);

                if (nodes[bitIndex] == null)
                {
                    OctreeLeafNode leaf = initLeaf(bitIndex, x, y, z, bitlevel + 1);
                    leaf.setChunkVal(x, y, z, val);
                }
                else
                {
                    nodes[bitIndex].Set(x, y, z, val, bitlevel + 1);
                }
            }
            else
            {
                int currentChildIndex = parent.GetChildIndex(this);
                OctreeChildNode newc = parent.initChild(currentChildIndex, x, y, z, bitlevel);
                newc.offsetBitNum = equalOffsetNum;
                bitlevel += equalOffsetNum;
                int bitIndex = BitHack.BitIndex(xcoord, ycoord, zcoord, bitlevel);
                newc.ReferChild(this, bitIndex);
                offsetBitNum -= (equalOffsetNum + 1);
                bitIndex = BitHack.BitIndex(x, y, z, bitlevel);
                OctreeLeafNode leaf = newc.initLeaf(bitIndex, x, y, z, bitlevel);
                leaf.offsetBitNum -= 1;
                leaf.setChunkVal(x, y, z, val);
            }

            return;
        }

        public override string ToString(int lz)
        {
            string lzstr = new string(' ', lz);
            string ret = base.ToString(lz) + "\n";

            for (int i = 0; i < 8; i++)
            {
                ret += lzstr + "Node" + i + " = " + (nodes[i] == null ? "null" : nodes[i].ToString(lz + 1)) + "\n";
            }

            return ret;
        }
    }
}
