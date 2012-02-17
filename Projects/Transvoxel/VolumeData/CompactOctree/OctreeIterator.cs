using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transvoxel.Math;

namespace Transvoxel.VolumeData.CompactOctree
{
    public class OctreeIterator<T> where T:new()
    {
        private OctreeNode<T> head;

        //last access
        private OctreeNode<T> node;
        private Vector3i pos;

        private int minlevel = 32;// - VolumeChunk.CHUNKBITS;

        public enum Axis
        { 
            X=0,Y=1,Z=2
        }

        public OctreeIterator(OctreeNode<T> startNode)
        {
            head = startNode;
            node = head;
        }

        public delegate void method(OctreeNode<T> n);
        public void Foreach(method m)
        {
            m(node);
        }

        public T GetValue()
        {
            return node.GetValue();
        }

        public void DirectAdressing(short x, short y, short z)
        {
            node = head.Get(x, y, z, minlevel);
        }

    /*    public void IncrementOne(Axis axis)
        {
            int w = node.GetPos()[(int)axis];
            int k = 0;

            while ((w & 1) == 1)
            {
                k++;
                w >>= 1;
            }
            k = (k+1) * 2;

            for (int i = 0; i < k / 2; i++)
            {
                node = node.GetParent();
            }

            if(axis == Axis.X)
                node = node.Get(w+1, node.GetPos().Y, node.GetPos().Z, minlevel);
            else if(axis == Axis.Y)
                node = node.Get(node.GetPos().X, w+1, node.GetPos().Z, minlevel);
            else if(axis == Axis.Z)
                node = node.Get(node.GetPos().X, node.GetPos().Y, w+1, minlevel);
        }*/
    }
}
