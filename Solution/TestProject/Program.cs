using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransvoxelXna.Helper;
using TransvoxelXna.VolumeData;

namespace TestProject
{
    // Just for some unit tests
    class Program
    {
        static void Main(string[] args)
        {
            VolumeDataBaseOctree octree = new VolumeDataBaseOctree();            

    
            
            
         /*   octree[1, 2, 1] = 15;
            octree[9, 1, 1] = 3;
            octree[18, 1, 1] = 3;

            Console.WriteLine(octree[-1, -2, -1]);

            Console.WriteLine(MathHelper.int2bitstr(1));
            Console.WriteLine(MathHelper.int2bitstr(2));
            Console.WriteLine(MathHelper.int2bitstr(1));
            Console.WriteLine();
            Console.WriteLine(MathHelper.int2bitstr(9));
            Console.WriteLine(MathHelper.int2bitstr(1));
            Console.WriteLine(MathHelper.int2bitstr(1));
            Console.WriteLine();
            Console.WriteLine(MathHelper.int2bitstr(18));
            Console.WriteLine(MathHelper.int2bitstr(1));
            Console.WriteLine(MathHelper.int2bitstr(1));*/


            for (int x = -10; x < 50; x++)
                for (int y = -10; y < 50; y++)
            {
                octree[x, y, 0] = (sbyte)(x * y);
            }

            int fail = 0;
            for (int x = -10; x < 50; x++)
                for (int y = -10; y < 50; y++)
            {
                if (octree[x, y, 0] != (sbyte)(x * y))
                {
                    fail++;
                }
            }
           
            Console.WriteLine(octree.ToString());
            Console.WriteLine(fail);

            Console.ReadLine();
        }
    }
}
