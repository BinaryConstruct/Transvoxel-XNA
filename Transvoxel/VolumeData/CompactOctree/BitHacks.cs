using System;

namespace TransvoxelXna.VolumeData.CompactOctree
{
    public static class BitHack
    {
        public static readonly uint[] MASK_LR = new uint[sizeof(int)*8]; //BitMask 1....0
        static BitHack()
        {
            MASK_LR[0] = 0x80000000;
            for (int i = 1; i < MASK_LR.Length; i++)
            {
                MASK_LR[i] = (MASK_LR[i - 1]>>1);
            }
        }

        public static uint Mask(int i)
        {
            return MASK_LR[i];
        }

        public static int BitIndex(int x, int y, int z, int bit)
        {
            return bitAt(x, bit) | (bitAt(y, bit) << 1) | (bitAt(z, bit) << 2);
        }

        public static int min(int a, int b)
        {
            if (a < b)
                return a;
            else
                return b;
        }

        public static int min(params int [] list)
        {
            int min = list[0];
            foreach (int i in list)
            {
                if (i < min)
                    min = i;
            }
            return min;
        }

        public static uint Mask(int i, int j)
        {
            uint ret = 0;
            for (int x = i; i < j+1; i++)
            {
                ret |= MASK_LR[i];
            }
            return ret;
        }

        public static string int2bitstr(int val)
        {
            string a = Convert.ToString(val,2);
            
            string zeros = "";
            for(int i=0;i<32-(a.Length);i++)
            {
                zeros += "0";
            }

            return zeros+a;
        }

        public static int bitAt(int val,int i)
        {
            return (val >> (sizeof(int)*8-i-1)) & 1;
        }

        /**
         * return num of equal bits from left to right, beginning with startIndex
         * */
        public static int cmpBit(int v1, int v2, int startIndex, int num)
        {
            for (int i = 0; i < num; i++)
            {
                if (!cmpBit(v1,v2,startIndex+i))
                {
                    return i;
                }
            }

            return num;
        }

        public static bool cmpBit(int v1, int v2, int bitindex)
        {
            return (v1 & Mask(bitindex)) == (v2 & Mask(bitindex));
        }
    }
}
