using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TransvoxelXna.Math;

namespace TransvoxelXna.Geometry
{
    public struct Vertex
    {
        public Vector3f pos;
    }

    public class Mesh
    {
        List<ushort> indizes;
        List<Vertex> vertizes;

        public Mesh()
        {
            indizes = new List<ushort>();
            vertizes = new List<Vertex>();
        }

        public void AddIndex(ushort i)
        {
            Console.WriteLine("a");
            indizes.Add(i);
        }

        public void AddVertex(Vertex v)
        {
            Console.WriteLine("b");
            vertizes.Add(v);   
        }

        public ushort[] GetIndizes()
        {
            return indizes.ToArray();
        }

        public Vertex[] GetVertizes()
        {
            return vertizes.ToArray();
        }

        internal int VertexCount()
        {
            return vertizes.Count;
        }
    }
}
