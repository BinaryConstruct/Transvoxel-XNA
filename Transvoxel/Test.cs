using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransvoxelXna.VolumeData;
using System.Diagnostics;
using TransvoxelXna.VolumeData.CompactOctree;

namespace TransvoxelXna
{
    public class Test
    {
        static void Main(string[] args)
        {
            IVolumeData octree = new CompactOctree();
            Stopwatch watch = new Stopwatch();
            int fail = 0;
            watch.Start();
            for (int i = -200; i < 200; i+=20)
            {
                for (int j = -200; j < 200; j += 50)
                {
                    for (int k = -200; k < 200; k += 50)
                    {

                        for (int x = -200; x < 200; x += 50)
                        {
                            for (int y = -200; y < 200; y += 50)
                            {
                                for (int z = -200; z < 200; z += 50)
                                {
                                    octree[x + i * 16, y + j * 16, z + k * 16] = (sbyte)(x + y + z);
                                }
                            }
                        }
                    }
                }
            }
            watch.Stop();
            Console.WriteLine("Write: " + watch.ElapsedMilliseconds);

            watch.Restart();
            for (int i = -200; i < 200; i += 50)
            {
                for (int j = -200; j < 200; j += 50)
                {
                    for (int k = -200; k < 200; k += 50)
                    {

                        for (int x = -200; x < 200; x += 50)
                        {
                            for (int y = -200; y < 200; y += 50)
                            {
                                for (int z = -200; z < 200; z += 50)
                                {
                                    var test = octree[x + i * 16, y + j * 16, z + k * 16];
                                    if (test != (sbyte)(x + y + z))
                                        fail++;
                                }
                            }
                        }
                    }
                }
            }
            watch.Stop();
            Console.WriteLine("Read: " + watch.ElapsedMilliseconds);
            Console.WriteLine(octree.ToString());
            Console.WriteLine(fail);
            Console.ReadLine();
        }
    }
}
