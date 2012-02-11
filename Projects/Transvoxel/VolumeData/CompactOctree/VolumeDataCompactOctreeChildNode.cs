using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Transvoxel.VolumeData.CompactOctree
{
    internal class OctreeChildNode : OctreeNode
    {
        private OctreeNode[] nodes = new OctreeNode[8]; //index: x+y*2+z*4 | x,y,z elementof {0,1}

        public OctreeChildNode(OctreeChildNode parent, int x, int y, int z)
            : base(parent, x, y, z)
        { }

        public OctreeChildNode initChild(int place, int x, int y, int z)
        {
            OctreeChildNode newChild = new OctreeChildNode(this, x, y, z);
            nodes[place] = newChild;
            return newChild;
        }

        public OctreeLeafNode initLeaf(int place, int x, int y, int z)
        {
            OctreeLeafNode leaf = new OctreeLeafNode(this, x, y, z);
            nodes[place] = leaf;
            leaf.level = level + offsetBitNum + 1;
            leaf.offsetBitNum = 29 - leaf.level;
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

        public OctreeNode GetNode(int x, int y, int z)
        {
            //Console.WriteLine("GetNode");
            int equalOffsetNum = EqualOffsetNum(x, y, z, level);
            int l = level;

            if (equalOffsetNum == offsetBitNum)
            {
                l += offsetBitNum;
            }
            else
            {
                return null;
            }

            int bitIndex = BitHack.BitIndex(x, y, z,l);

            if (nodes[bitIndex] == null)
            {
                return null;
            }

            return nodes[bitIndex];
        }

        internal override sbyte Get(int x, int y, int z)
        {
            OctreeNode n = GetNode(x,y,z);
            return (n==null?(sbyte)0:n.Get(x, y, z));
        }

        internal override void Set(int x, int y, int z, sbyte val)
        {
            int equalOffsetNum = EqualOffsetNum(x, y, z, level);

            if (equalOffsetNum == offsetBitNum)
            {
                int bitIndex = BitHack.BitIndex(x, y, z, level + offsetBitNum);

                if (nodes[bitIndex] == null)
                {
                    //Node doesn't exist - create it
                    OctreeLeafNode leaf = initLeaf(bitIndex, x, y, z);
                    leaf.setChunkVal(x, y, z, val);
                }
                else
                {
                    //Node exists - refer Set call
                    nodes[bitIndex].Set(x, y, z, val);
                }
            }
            else
            {
                //Create Node
                OctreeChildNode newc = parent.initChild(parent.GetChildIndex(this), x, y, z);
                newc.offsetBitNum = equalOffsetNum;
                newc.level = level;

                //Add this as child to the new Node
                int bitIndex = BitHack.BitIndex(xcoord, ycoord, zcoord, newc.level+newc.offsetBitNum);
                newc.ReferChild(this, bitIndex);
                offsetBitNum -= (equalOffsetNum + 1);
                level = newc.level + newc.offsetBitNum + 1;

                bitIndex = BitHack.BitIndex(x, y, z, newc.level + newc.offsetBitNum);
                OctreeLeafNode leaf = newc.initLeaf(bitIndex, x, y, z);
                leaf.setChunkVal(x, y, z, val);
            }

            return;
        }

        internal override bool HasChilds()
        {
            return (nodes[0] != null || nodes[1] != null || nodes[2] != null || nodes[3] != null || nodes[4] != null || nodes[5] != null || nodes[6] != null || nodes[7] != null);
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
