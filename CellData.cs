using System;

namespace VoxelTest
{
    public class CellData
    {
        public readonly byte[] VertexIndex;
        public readonly byte GeometryCounts;

        public CellData(int size, byte geometryCounts, byte[] vertexIndex)
        {
            VertexIndex = vertexIndex;
            Array.Resize(ref VertexIndex, size);
            GeometryCounts = geometryCounts;
        }

        public int GetVertexCount()
        {
            return (GeometryCounts >> 4);
        }

        public int GetTriangleCount()
        {
            return (GeometryCounts & 0x0F);
        }
    }
}