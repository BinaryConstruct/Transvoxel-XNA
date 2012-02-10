using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Transvoxel.Math;

namespace Transvoxel.Geometry
{
    //for later stuff we use a custom vertex structure struct Vertex
    //for now keep it simple as Vector3f
    /*public struct Vertex
    {
        public Vector3f pos;
    }*/

    public class Mesh
    {
        List<ushort> indizes;
        List<Vector3f> vertizes;

        public Mesh()
        {
            indizes = new List<ushort>();
            vertizes = new List<Vector3f>();
        }

        public void AddIndex(ushort i)
        {
            indizes.Add(i);
        }

        public void AddVertex(Vector3f v)
        {
            vertizes.Add(v);   
        }

        public ushort[] GetIndizes()
        {
            return indizes.ToArray();
        }

        public Vector3f[] GetVertizes()
        {
            return vertizes.ToArray();
        }

        internal int VertexCount()
        {
            return vertizes.Count;
        }
    }
}
