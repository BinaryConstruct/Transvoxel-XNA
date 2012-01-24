using System;
using TransvoxelXna.Helper;
using System.Diagnostics;

namespace TransvoxelXna.VolumeData
{
    public class VolumeDataBaseOctree : VolumeDataBase
    {
        private OctreeChildNode head;

        public VolumeDataBaseOctree()
        {
            head = new OctreeChildNode(null);
            head.name = "head";
        }

        public override sbyte this[int x, int y, int z]
        {
            get
            {
                return head.Sample(x,y,z,0);
            }

            set
            {
               // Console.WriteLine("------ CALL SET ---------");
               // Console.WriteLine("<Head>");
                head.Set(x, y, z, value,0);
               // Console.WriteLine("------ RETN SET ---------");
            }
        }

        public override string ToString()
        {
            return "NumLeafNodes: "+ head.numLeafNodes()+ "\n"+head.ToString(0);
        }
    }

    internal abstract class OctreeNode
    {
        internal int offsetBitNum = 0;
        internal int xoff = 0; //shifted to lsb
        internal int yoff = 0;
        internal int zoff = 0;
        internal OctreeChildNode parent;
        
        public string name = "child";
        public short serial;

        public OctreeNode(OctreeChildNode parent)
        {
            this.parent = parent;
            serial = (short)new Random().Next();
        }

        public abstract sbyte Sample(int x, int y, int z,int bitlevel);
        public abstract void Set(int x, int y, int z, sbyte val, int bitlevel);

        public virtual string ToString(int lz)
        {
            string lzstr = "";
            for (int i = 0; i < lz; i++)
            {
                lzstr += " ";
            }

            return lzstr+"< " + this.GetType().ToString() + " > offsetBitNum:"+offsetBitNum;
        }
    }

    internal class OctreeChildNode : OctreeNode
    {
        private OctreeNode[] nodes=new OctreeNode[8]; //index: x+y*2+z*4 | x,y,z elementof {0,1}

        public OctreeChildNode(OctreeChildNode parent)
            : base(parent)
        { }

        public int numLeafNodes()
        {
            int num = 0;
            for (int i = 0; i < 8; i++)
            {
                if (nodes[i] is OctreeLeafNode)
                {
                    num++;
                }
                else if (nodes[i] is OctreeChildNode)
                {
                    OctreeChildNode n = (OctreeChildNode)nodes[i];
                    num += n.numLeafNodes();
                }
            }

            return num;
        }

        public override string ToString(int lz)
        {
            string lzstr = "";
            for (int i = 0; i < lz+1;i++ )
            {
                lzstr += " ";
            }

            string ret = base.ToString(lz)+"\n";

            for(int i=0;i<8;i++)
            {
                ret += lzstr+"Node"+i+" = "+(nodes[i]==null?"null":nodes[i].ToString(lz+1))+"\n";
            }

            return ret;
        }

        public OctreeChildNode initChild(int place, int x, int y, int z, int bitlevel)
        {
            OctreeChildNode newChild  = new OctreeChildNode(this);
            newChild.xoff = x;
            newChild.yoff = y;
            newChild.zoff = z;

            newChild.offsetBitNum = sizeof(int) * 8 - bitlevel - VolumeChunk.CHUNKBITS;
            

            nodes[place] = newChild;
            return newChild;
        }

        public OctreeLeafNode initLeaf(int place,int x,int y,int z,int bitlevel)
        {
            OctreeLeafNode leaf = new OctreeLeafNode(this);
            leaf.xoff = x;
            leaf.yoff = y;
            leaf.zoff = z;

            leaf.offsetBitNum = sizeof(int) * 8 - bitlevel - VolumeChunk.CHUNKBITS;
            
            
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
            return -1;
        }

        public void ReferChild(OctreeNode n,int i)
        {
            nodes[i] = n;
            n.parent = this; //?
        }

        public override sbyte Sample(int x, int y, int z, int bitlevel)
        {
            if (offsetBitNum == 0 ||
                (MathHelper.cmpBit(xoff, x, bitlevel, offsetBitNum) == offsetBitNum &&
                MathHelper.cmpBit(yoff, y, bitlevel, offsetBitNum) == offsetBitNum &&
                MathHelper.cmpBit(zoff, z, bitlevel, offsetBitNum) == offsetBitNum))
            {
                bitlevel += offsetBitNum;
            }
            else
            {
                return 0;
            }

            int xx = MathHelper.bitAt(x, bitlevel);
            int yy = MathHelper.bitAt(y, bitlevel);
            int zz = MathHelper.bitAt(z, bitlevel);

            if (nodes[xx + yy * 2 + zz * 4] == null)
            {
                Console.WriteLine("Node doesn't exist");
                return 0;
            }

            return nodes[xx + yy * 2 + zz * 4].Sample(x, y, z, bitlevel + 1);
        }

        public override void Set(int x, int y, int z, sbyte val, int bitlevel)
        {
            int equalX = MathHelper.cmpBit(xoff, x, bitlevel, offsetBitNum);
            int equalY = MathHelper.cmpBit(yoff, y, bitlevel, offsetBitNum);
            int equalZ = MathHelper.cmpBit(zoff, z, bitlevel, offsetBitNum);

            int equalOffsetNum = MathHelper.min(equalX, MathHelper.min(equalY, equalZ));

            //if (offsetBitNum == 0 || (equalX == offsetBitNum && equalY == offsetBitNum && equalZ == offsetBitNum))
            if (equalOffsetNum == offsetBitNum)
            {
                //Console.WriteLine("All offsetBitNum == 0 or all offsets are equal");

                bitlevel += offsetBitNum;

                int xx = MathHelper.bitAt(x, bitlevel);
                int yy = MathHelper.bitAt(y, bitlevel);
                int zz = MathHelper.bitAt(z, bitlevel);

                if (nodes[xx + yy * 2 + zz * 4] == null)
                {
                    //Console.WriteLine("Child: " + (xx + yy * 2 + zz * 4)+" doesn't exist - Create LeafChild");
                    OctreeLeafNode leaf = initLeaf(xx + yy * 2 + zz * 4,x,y,z, bitlevel + 1);
                    leaf.setChunkVal(x, y, z, val);
                    //leaf.Set(x, y, z, val,bitlevel);
                }
                else
                {
                    //Console.WriteLine("Child: " + (xx + yy * 2 + zz * 4) + " does exist - Go to next level in tree");
                    nodes[xx + yy * 2 + zz * 4].Set(x, y, z, val,bitlevel+1);
                }
            }
            else 
            {
                //Console.WriteLine("Offsets are not equal");
                //int equalOffsetNum = MathHelper.min(equalX, MathHelper.min(equalY, equalZ));
                int currentChildIndex = parent.GetChildIndex(this);
                OctreeChildNode newc = parent.initChild(currentChildIndex,x,y,z,bitlevel);
                newc.offsetBitNum = equalOffsetNum;
                bitlevel += equalOffsetNum;
                
                int xx = MathHelper.bitAt(xoff, bitlevel);
                int yy = MathHelper.bitAt(yoff, bitlevel);
                int zz = MathHelper.bitAt(zoff, bitlevel);

                newc.ReferChild(this, xx + yy * 2 + zz * 4);
                offsetBitNum -= (equalOffsetNum+1);

                xx = MathHelper.bitAt(x, bitlevel);
                yy = MathHelper.bitAt(y, bitlevel);
                zz = MathHelper.bitAt(z, bitlevel);

                OctreeLeafNode leaf = newc.initLeaf(xx + yy * 2 + zz * 4,x,y,z,bitlevel);
                leaf.offsetBitNum = 32 - bitlevel - 1 - VolumeChunk.CHUNKBITS;
                leaf.setChunkVal(x, y, z, val);
                //leaf.Set(x, y, z, val, bitlevel);
            }
            
            return;
        }
    }

    internal class OctreeLeafNode : OctreeNode
    {
        private VolumeChunk chunk = new VolumeChunk();
        private static readonly int shiftval = sizeof(int) - VolumeChunk.CHUNKBITS;

        public OctreeLeafNode(OctreeChildNode parent)
            : base(parent)
        { }

        public override string ToString(int lz)
        {
            string lzstr = "";
            for (int i = 0; i < lz; i++)
            {
                lzstr += " ";
            }

            string ret = lzstr+base.ToString(lz);

            return ret;
        }

        public override sbyte Sample(int x, int y, int z, int bitlevel)
        {
            return chunk[x, y, z];
        }

        public void setChunkVal(int x, int y, int z, sbyte val)
        {
            chunk[x, y, z] = val;
        }

        public override void Set(int x, int y, int z, sbyte val,int bitlevel)
        {
            int equalX = MathHelper.cmpBit(xoff, x, bitlevel, offsetBitNum);
            int equalY = MathHelper.cmpBit(yoff, y, bitlevel, offsetBitNum);
            int equalZ = MathHelper.cmpBit(zoff, z, bitlevel, offsetBitNum);

            int equalOffsetNum = MathHelper.min(equalX, MathHelper.min(equalY, equalZ));

            if (equalOffsetNum == offsetBitNum)
            {
                //existing leaf
                chunk[x, y, z] = val;
            }
            else
            { 
                //new leaf

                int currentChildIndex = parent.GetChildIndex(this);
                OctreeChildNode newc = parent.initChild(currentChildIndex, x, y, z, bitlevel);
                newc.offsetBitNum = equalOffsetNum;
                bitlevel += equalOffsetNum;

                int xx = MathHelper.bitAt(xoff, bitlevel);
                int yy = MathHelper.bitAt(yoff, bitlevel);
                int zz = MathHelper.bitAt(zoff, bitlevel);

                newc.ReferChild(this, xx + yy * 2 + zz * 4);
                offsetBitNum -= (equalOffsetNum + 1);

                xx = MathHelper.bitAt(x, bitlevel);
                yy = MathHelper.bitAt(y, bitlevel);
                zz = MathHelper.bitAt(z, bitlevel);

                OctreeLeafNode leaf = newc.initLeaf(xx + yy * 2 + zz * 4, x, y, z, bitlevel);
                leaf.offsetBitNum = 32 - bitlevel-1-VolumeChunk.CHUNKBITS;
                leaf.chunk[x, y, z] = val;
            }
            //todo
            //chunk[x, y, z] = val;
        }
    }
}
