using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics;
using TransvoxelXna.VolumeData;
using TransvoxelXna.VolumeData.CompactOctree;

namespace UnitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            IVolumeData octree = new CompactOctree();
            Stopwatch watch = new Stopwatch();
            int fail = 0;
            // full test
            watch.Start();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    for (int k = 0; k < 8; k++)
                    {

                        for (int x = 0; x < 16; x++)
                        {
                            for (int y = 0; y < 16; y++)
                            {
                                for (int z = 0; z < 16; z++)
                                {
                                    octree[x + i * 16, y + j * 16, z + k * 16] = (sbyte)(x + y + z);
                                }
                            }
                        }
                    }
                }
            }
            watch.Stop();
            Debug.WriteLine("Write: " + watch.ElapsedMilliseconds);

            watch.Restart();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    for (int k = 0; k < 8; k++)
                    {

                        for (int x = 0; x < 16; x++)
                        {
                            for (int y = 0; y < 16; y++)
                            {
                                for (int z = 0; z < 16; z++)
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
            Debug.WriteLine("Read: " + watch.ElapsedMilliseconds);
            Debug.WriteLine(octree.ToString());
            Debug.WriteLine(fail);
            Assert.IsTrue(fail != 0, "Failed");
            Assert.Inconclusive("Verify Correctness");
        }
    }
}
