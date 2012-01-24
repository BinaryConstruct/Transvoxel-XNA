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

            octree[0, 0, 0] = 10;
            Console.WriteLine(octree[0, 0, 0]);


            Console.ReadLine();
        }
    }
}
