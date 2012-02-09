using System.Diagnostics;
using TransvoxelXna.Math;
using TransvoxelXna.VolumeData;
using TransvoxelXna.VolumeData.CompactOctree;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTests
{
    /// <summary>
    ///This is a test class for CompactOctreeTest and is intended
    ///to contain all CompactOctreeTest Unit Tests
    ///</summary>
    [TestClass()]
    public class CompactOctreeTest
    {

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        // 
        //You can use the following additional attributes as you write your tests:
        //
        //Use ClassInitialize to run code before running the first test in the class
        //[ClassInitialize()]
        //public static void MyClassInitialize(TestContext testContext)
        //{
        //}
        //
        //Use ClassCleanup to run code after all tests in a class have run
        //[ClassCleanup()]
        //public static void MyClassCleanup()
        //{
        //}
        //
        //Use TestInitialize to run code before running each test
        //[TestInitialize()]
        //public void MyTestInitialize()
        //{
        //}
        //
        //Use TestCleanup to run code after each test has run
        //[TestCleanup()]
        //public void MyTestCleanup()
        //{
        //}
        //
        #endregion


        /// <summary>
        ///A test for CompactOctree Constructor
        ///</summary>
        [TestMethod()]
        public void CompactOctreeConstructorTest()
        {
            IVolumeData octree = new CompactOctree();
            Stopwatch watch = new Stopwatch();
            int fail = 0;

            //for (int x = -10; x < 50; x += 2)
            //    for (int y = -10; y < 50; y += 3)
            //    {
            //        octree[x, y, 0] = (sbyte)(x * y);
            //    }
            //for (int x = -10; x < 50; x += 2)
            //    for (int y = -10; y < 50; y += 3)
            //    {
            //        if (octree[x, y, 0] != (sbyte)(x * y))
            //        {
            //            fail++;
            //        }
            //    }

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
                                    octree[x + i*16, y + j*16, z + k*16] = (sbyte)(x + y + z);
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
                                    var test = octree[x + i*16, y + j*16, z + k*16];
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
            //Random rnd = new Random();

            //watch.Start();

            ////for (int i = 0; i < 100000000; i++)
            //{
            //    //BitHack.bitAt(rnd.Next(), rnd.Next(0, 31));
            //}

            //watch.Stop();
            //Debug.WriteLine(watch.ElapsedMilliseconds);
        }

        /// <summary>
        ///A test for Item
        ///</summary>
        [TestMethod()]
        public void ItemTest()
        {
            CompactOctree target = new CompactOctree(); // TODO: Initialize to an appropriate value
            int x = 0; // TODO: Initialize to an appropriate value
            int y = 0; // TODO: Initialize to an appropriate value
            int z = 0; // TODO: Initialize to an appropriate value
            sbyte expected = 0; // TODO: Initialize to an appropriate value
            sbyte actual;
            target[x, y, z] = expected;
            actual = target[x, y, z];
            Assert.AreEqual(expected, actual);
            Assert.Inconclusive("Verify the correctness of this test method.");
        }
    }
}
