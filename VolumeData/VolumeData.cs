using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using VoxelStuff.helper;
using VoxelTest.helper;

namespace VoxelStuff.VolumeData
{
    public class VolumeDataOctree : IVolumeData
    {
        private OctreeNode head;

        public override sbyte this[int x, int y, int z]
        {
            get
            {
                //convert x,y,z to sign+abs?
                return head==null?(sbyte)0:head.Sample(x,y,z,0);
            }

            set
            {
                if (head == null)
                {
                    head = new OctreeLeafNode();
                }

                    
                //data[x / VolumeChunk.CHUNKSIZE, y / VolumeChunk.CHUNKSIZE, z / VolumeChunk.CHUNKSIZE][x, y, z] = value;
            }
        }
    }

    internal abstract class OctreeNode
    {
        int offsetBitNum = 0;
        int xoff = 0; //shifted to lsb
        int yoff = 0;
        int zoff = 0;

        public abstract sbyte Sample(int x, int y, int z,int bitlevel);
        public abstract void Set(int x, int y, int z, sbyte val);
        
    }

    internal class OctreeChildNode : OctreeNode
    {
        private OctreeNode[] nodes; //index: x+y*2+z*4 | x,y,z elementof {0,1}

        public override sbyte Sample(int x, int y, int z, int bitlevel)
        {
            int xx = MathHelper.bitAt(x, bitlevel);
            int yy = MathHelper.bitAt(y, bitlevel);
            int zz = MathHelper.bitAt(z, bitlevel);

            if (nodes[xx + yy * 2 + zz * 4] == null)
            {
                Console.WriteLine("Node doesn't exist");
                return 0;
            }

            return nodes[xx+yy*2+zz*4].Sample(x,y,z,bitlevel+1);
        }

        public override void Set(int x, int y, int z, sbyte val)
        {
            
        }
    }

    internal class OctreeLeafNode : OctreeNode
    {
        private VolumeChunk chunk = new VolumeChunk();
        private static readonly int shiftval = sizeof(int) - VolumeChunk.CHUNKBITS;

        public override sbyte Sample(int x, int y, int z, int bitlevel)
        {
            //bitlevel == shiftval ??

            x <<= shiftval; x >>= shiftval; //delete non chunk bits
            y <<= shiftval; y >>= shiftval;
            z <<= shiftval; z >>= shiftval;

            return chunk[x, y, z];
        }

        public override void Set(int x, int y, int z, sbyte val)
        { 
            x <<= shiftval; x >>= shiftval; //delete non chunk bits
            y <<= shiftval; y >>= shiftval;
            z <<= shiftval; z >>= shiftval;
            chunk[x, y, z] = val;   
        }
    }
}
