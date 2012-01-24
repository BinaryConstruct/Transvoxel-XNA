using System.Collections.Generic;
using Microsoft.Xna.Framework;
using TransvoxelXna.Helper;
using TransvoxelXna.VolumeData;

namespace TransvoxelXna
{
    public class Implementation
    {
        public void generateNegativeXTransitionCells(
            IVolumeData samples,
            Vector3 offset,
            Vector3i min,
            float cellSize,
            byte lodIndex,
            List<Vertex> verts,
            List<int> indices)
        {
            int spacing = 1 << lodIndex; // Spacing between low-res corners.

            Vector3i origin = min + new Vector3i(0, 0, 16);
            Vector3i xAxis = new Vector3i(0, 0, -1);
            Vector3i yAxis = new Vector3i(0, 1, 0);
            Vector3i zAxis = new Vector3i(1, 0, 0);

            TransitionCache cache = new TransitionCache();

            for (int y = 0; y < 16; ++y)
            {
                for (int x = 0; x < 16; ++x)
                {
                    Vector3i p = (origin + xAxis * x + yAxis * y) * spacing;
                    byte directionMask = (byte)((x > 0 ? 1 : 0) | ((y > 0 ? 1 : 0) << 1));
                    //Transvoxel.PolygonizeTransitionCell(offset, p, xAxis, yAxis, zAxis, x, y, cellSize, lodIndex, 0, directionMask, samples, verts, indices, cache);
                }
            }
        }
    }
}