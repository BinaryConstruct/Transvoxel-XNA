﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Transvoxel.VolumeData.CompactOctree
{
    public class OctreeLeafNode : OctreeNodeBase
    {
        private VolumeChunk chunk = new VolumeChunk();

        public OctreeLeafNode(OctreeChildNode parent, int x, int y, int z)
            : base(parent, x, y, z)
        { }

        /*public override OctreeNode GetNode(int x, int y, int z)
        {
            return this;
        }*/

        internal override void Set(int x, int y, int z, sbyte val)
        {
            //Console.WriteLine("SetLeaf "+level+" "+offsetBitNum);
            int equalOffsetNum = EqualOffsetNum(x, y, z);

            if (equalOffsetNum == offsetBitNum)
            {
                //existing leaf - set the value on this node                

                if (val == 0 && chunk.ContainsOnlyZero())
                {
                    //Destroy this node
                }
                else
                {
                    setChunkVal(x, y, z, val);
                }
            }
            else
            {
                if (val == 0)   //don't save zeros
                    return;

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
                //leaf.chunk[x, y, z] = val;

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
