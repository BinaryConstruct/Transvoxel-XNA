using Microsoft.Xna.Framework;

namespace TransvoxelXna.SurfaceExtractor
{
    public struct Vertex
    {
        public Vector3 Primary;
        public Vector3 Secondary;
        public Vector3 Normal;
        public byte Near;
    }
}