using System;

namespace TransvoxelXna.helper
{
    public class MathHelper
    {
        public static readonly uint[] MASK_LR = new uint[sizeof(int)*8]; //BitMask 1....0
        static MathHelper()
        {
            MASK_LR[0] = 0x80000000;
            for (int i = 1; i < MASK_LR.Length; i++)
            {
                MASK_LR[i] = (MASK_LR[i - 1]>>1);
                Console.WriteLine(int2bitstr(MASK_LR[i]));
            }
        }

        public static uint Mask(int i)
        {
            return MASK_LR[i];
        }

        public static string int2bitstr(uint val)
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
            return (val >> i) & 1;
        }

        /**
         * return num of equal bits from left to right, beginning with startIndex
         * */
        public static int cmpBit(int v1, int v2, int startIndex, int num)
        {
            for (int i = 0; i < num; i++)
            {
                if (cmpBit(v1,v2,startIndex+i))
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
