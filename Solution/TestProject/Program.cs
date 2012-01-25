using System;

using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransvoxelXna.VolumeData.CompactOctree;
using TransvoxelXna.VolumeData;
using TransvoxelXna.VolumeData.CompactOctree;
using System.Diagnostics;

namespace TestProject
{
    // Just for some unit tests
    class Program
    {
        static void Main(string[] args)
        {
            CompactOctree octree = new CompactOctree();       
    


            for (int x = -10; x < 50; x+=2)
                for (int y = -10; y < 50; y+=3)
            {
                octree[x, y, 0] = (sbyte)(x * y);
            }

            int fail = 0;
            for (int x = -10; x < 50; x+=2)
                for (int y = -10; y < 50; y+=3)
            {
                if (octree[x, y, 0] != (sbyte)(x * y))
                {
                    fail++;
                }
            }
           
            Console.WriteLine(octree.ToString());
            Console.WriteLine(fail);
            Random rnd = new Random();
            Stopwatch watch = new Stopwatch();
            watch.Start();

            for(int i=0;i<100000000;i++)
            {
                BitHack.bitAt(rnd.Next(),rnd.Next(0,31));
            }

            watch.Stop();
            Console.WriteLine(watch.ElapsedMilliseconds);

            Console.ReadLine();
        }
    }
}
