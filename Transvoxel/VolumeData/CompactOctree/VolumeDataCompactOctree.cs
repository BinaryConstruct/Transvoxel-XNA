using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TransvoxelXna.VolumeData.CompactOctree
{
    public class CompactOctree : VolumeDataBase
    {
        private OctreeChildNode head;

        public CompactOctree()
        {
            head = new OctreeChildNode(null, 0, 0, 0, sizeof(int) * 8 - VolumeChunk.CHUNKBITS);
        }

        public override sbyte this[int x, int y, int z]
        {
            get
            {
                return head.Get(x, y, z, 0);
            }

            set
            {
                head.Set(x, y, z, value, 0);
            }
        }

        public override string ToString()
        {
            return head.ToString(0);
        }
    }
}
