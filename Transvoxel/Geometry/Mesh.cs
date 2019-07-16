﻿using System;
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
        private List<ushort> indices;
        private List<Vector3f> vertices;
        private List<Vector3f> normals; 

        public Mesh()
        {
            indices = new List<ushort>();
            vertices = new List<Vector3f>();
            normals = new List<Vector3f>();
        }

        public void AddIndex(ushort i)
        {
            indices.Add(i);
        }

        public void AddVertex(Vector3f v, Vector3f normal)
        {
            vertices.Add(v);
            normals.Add(normal);
        }

        public ushort[] GetIndices()
        {
            return indices.ToArray();
        }

        public Vector3f[] GetVertices()
        {
            return vertices.ToArray();
        }

        public Vector3f[] GetNormals()
        {
            return normals.ToArray();
        }

        public List<Vector3f> Vertices
        {
            get { return vertices; }
        }

        public List<Vector3f> Normals
        {
            get { return normals; }
        }

        public List<ushort> Indices
        {
            get { return indices; }
        }

        public float[] GetVerticesFloatArray()
        {
            float[] arr = new float[vertices.Count * 3];
            int i = 0;
            foreach (Vector3f v in vertices)
            {
                arr[i + 0] = v.X;
                arr[i + 1] = v.Y;
                arr[i + 2] = v.Z;
                i++;
            }
            return arr;
        }

        internal ushort LatestAddedVertIndex()
        {
            return (ushort)(vertices.Count-1);
        }
    }
}
