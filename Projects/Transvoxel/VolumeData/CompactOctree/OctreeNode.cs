using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transvoxel.Math;
using System.Diagnostics;

namespace Transvoxel.VolumeData.CompactOctree
{
    public class OctreeNode<T> where T:new()
    {
        internal int offsetBitNum = 0;  //equal bits for all subnodes
        internal int level = 0;         //octree depth (before the offset bits)
        internal int xcoord = 0;
        internal int ycoord = 0;
        internal int zcoord = 0;

        internal OctreeNode<T> parent;
        internal T value = default(T);
        private OctreeNode<T>[] nodes = null;//new OctreeNode[8]; //index: x+y*2+z*4 | x,y,z elementof {0,1}

        internal OctreeNode(OctreeNode<T> parent, int x, int y, int z)
        {
            this.parent = parent;
            xcoord = x;
            ycoord = y;
            zcoord = z;
            offsetBitNum = 0;
            level = 0;
        }

        public OctreeNode<T> initChild(int place, int x, int y, int z)
        {
            OctreeNode<T> newChild = new OctreeNode<T>(this, x, y, z);
            SetChild(newChild, place);//nodes[place] = newChild;
            return newChild;
        }

        public OctreeNode<T> initLeaf(int place, int x, int y, int z)
        {
            OctreeNode<T> leaf = new OctreeNode<T>(this, x, y, z);
            SetChild(leaf, place); //nodes[place] = leaf;
            leaf.level = level + offsetBitNum + 1;
            leaf.offsetBitNum = 32- VolumeChunk.CHUNKBITS - leaf.level;

            leaf.value = new T();
            return leaf;
        }

        

        internal OctreeNode<T> Get(int x, int y, int z, int minlevel)
        {
            if (GetLevel() == minlevel)
                return this;

            int equalOffsetNum = EqualOffsetNum(x, y, z);
            int l = level;

            if (equalOffsetNum == offsetBitNum)
            {
                l += offsetBitNum;
            }
            else
            {
                return null;
            }

            int bitIndex = BitHack.BitIndex(x, y, z, l);

            if (!HasChilds() || nodes[bitIndex] == null)
                return null;

            return nodes[bitIndex].Get(x,y,z,minlevel);
        }

        internal T Set(int x, int y, int z, int minlevel)
        {
            

            int equalOffsetNum = EqualOffsetNum(x, y, z);

            if (equalOffsetNum == offsetBitNum)
            {
                int bitIndex = BitHack.BitIndex(x, y, z, level + offsetBitNum);

                if (GetLevel() == minlevel)
                {
                    return value;
                }

                if (!HasChilds() || nodes[bitIndex] == null)
                {
                    //Node doesn't exist - create it
                    OctreeNode<T> leaf = initLeaf(bitIndex, x, y, z);

                    return leaf.value;
                    //leaf.value[x, y, z] = val;
                    //leaf.setChunkVal(x, y, z, val);
                }
                else
                {
                    //Node exists - refer Set call
                    OctreeNode<T> n = nodes[bitIndex];

                 //   if (n.GetLevel() == minlevel)
                 //       return n.value;

                    return n.Set(x, y, z,minlevel);
                }
            }
            else
            {
                //Create Node
                OctreeNode<T> newc = parent.initChild(parent.GetChildIndex(this), x, y, z);
                newc.offsetBitNum = equalOffsetNum;
                newc.level = level;

                //Add this as child to the new Node
                int bitIndex = BitHack.BitIndex(xcoord, ycoord, zcoord, newc.level+newc.offsetBitNum);
                newc.SetChild(this, bitIndex);
                offsetBitNum -= (equalOffsetNum + 1);
                level = newc.level + newc.offsetBitNum + 1;

                bitIndex = BitHack.BitIndex(x, y, z, newc.level + newc.offsetBitNum);
                OctreeNode<T> leaf = newc.initLeaf(bitIndex, x, y, z);
                //leaf.setChunkVal(x, y, z, val);
                //leaf.value[x, y, z] = val;

                return leaf.value;
            }
        }

        internal bool HasChilds()
        {
            return nodes != null;
        }

        public string ToStringA(int lz)
        {
            string lzstr = new string(' ', lz);
            string ret = ToStringB(lz) + "\n";

            if (!HasChilds())
                return ret;
            
            for (int i = 0; i < 8; i++)
            {
                ret += lzstr + "Node" + i + " = " + (!HasChilds() || nodes[i] == null ? "null" : nodes[i].ToStringA(lz + 1)) + "\n";
            }

            return ret;
        }

        public string ToStringB(int lz)
        {
            string lzstr = new string(' ', lz);
            return lzstr + "< " + this.GetType().ToString() + " > offsetBitNum:" + offsetBitNum+" lvl:"+GetLevel()+" lod:"+GetLevelOfDetail();
        }

        internal int GetLevel()
        {
            return level + offsetBitNum;
        }

        // from 1 to infinity
        public int GetLevelOfDetail()
        {
            return (32+1-VolumeChunk.CHUNKBITS) - GetLevel();
        }

        public Vector3i GetPos()
        {
            int mask = (int)BitHack.Mask(0, level-1);
            int x = xcoord & mask;
            int y = ycoord & mask;
            int z = zcoord & mask;
            return new Vector3i(x,y,z);
        }

        public int Size()
        { 
            return (1<<(GetLevelOfDetail()-1))*VolumeChunk.CHUNKSIZE;
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

        public int GetChildIndex(OctreeNode<T> n)
        {
            for (int i = 0; i < nodes.Length; i++)
            {
                if (n == nodes[i])
                    return i;
            }

            //Throws index out of bounds, because GetChildIndex always adresses an array
            return -1;
        }

        internal void SetChild(OctreeNode<T> n, int i)
        {
            if (nodes == null)
            {
                nodes = new OctreeNode<T>[8];
            }
            nodes[i] = n;
            n.parent = this;
        }

        public OctreeNode<T>[] GetChilds()
        {
            return nodes;
        }

        public delegate void foreachmeth(OctreeNode<T> n);
        public void Foreach(foreachmeth meth)
        {
            if (HasChilds())
            {
                for (int i = 0; i < 8; i++)
                {
                    meth(GetChilds()[i]);
                }
            }
        }
    }
}
